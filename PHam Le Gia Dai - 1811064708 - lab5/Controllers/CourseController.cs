using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PHam_Le_Gia_Dai___1811064708___lab5.Models;

namespace PHam_Le_Gia_Dai___1811064708___lab5.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course
        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            using (var context = new ApplicationDbContext())
            {
                var course = new Course();
                //course.Categories = context.Categories.ToList();
                ViewBag.Categories = context.Categories.ToList();
                return View("Create", course);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course course)
        {
            using (var context = new ApplicationDbContext())
            {
                ModelState.Remove("LecturerId");
                ModelState.Remove("ApplicationUser");
                ModelState.Remove("Attendances");
                ModelState.Remove("Category");

                if (!ModelState.IsValid)
                {
                    ViewBag.Categories = context.Categories.ToList();
                    return View("Create", course);
                }

                var user = System.Web.HttpContext
                    .Current
                    .GetOwinContext()
                    .GetUserManager<ApplicationUserManager>()
                    .FindById(User.Identity.GetUserId());
                course.LecturerId = user.Id;
                //course.ApplicationUser = user;
                course.DateTime = DateTime.Parse(course.DateTime.ToString());
                course.Category = context.Categories.Find(course.CategoryId);
                context.Courses.Add(course);
                context.SaveChanges();

                return RedirectToAction("Index", "Course");
            }
        }

        [HttpGet]
        public ActionResult Index()
        {
            using (var context = new ApplicationDbContext())
            {
                var curr = User.Identity.GetUserId();
                var newCourses = context.Courses.Where(u => u.DateTime > DateTime.Now && u.LecturerId == curr)
                    .OrderBy(u => u.DateTime).ToList();
                var result = new List<CourseListResource>();
                foreach (var course in newCourses)
                {
                    var user = System.Web.HttpContext.Current
                        .GetOwinContext()
                        .GetUserManager<ApplicationUserManager>()
                        .FindById(course.LecturerId);
                    result.Add(new CourseListResource
                    {
                        Id = course.Id,
                        DateTime = course.DateTime,
                        Category = context.Categories.Find(course.CategoryId).Name,
                        CourseName = course.Name,
                        Lecturer = user.Name,
                        Place = course.Place
                    });
                }

                return View(result);
            }
        }

        [HttpGet]
        public ActionResult Attending()
        {
            using (var context = new ApplicationDbContext())
            {
                var userId = User.Identity.GetUserId();
                var listAtds = context.Attendances.Where(u => u.AttendanceId == userId).ToList();
                var course = new List<Course>();
                foreach (var attendance in listAtds)
                {
                    var objCourse = attendance.Course;
                    objCourse.ApplicationUser = System.Web.HttpContext.Current
                        .GetOwinContext()
                        .GetUserManager<ApplicationUserManager>()
                        .FindById(objCourse.LecturerId);
                    var cate = context.Categories.FirstOrDefault(u => u.Id == objCourse.CategoryId);
                    objCourse.Category = cate;
                    course.Add(objCourse);
                }

                return View(course);
            }
        }

        [HttpGet]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            using (var context = new ApplicationDbContext())
            {
                var date = DateTime.UtcNow.AddHours(7);
                var courses = context.Courses
                    .Where(u => (u.LecturerId == userId)
                                & (u.DateTime >= date))
                    .ToList();
                var user = System.Web.HttpContext.Current
                    .GetOwinContext()
                    .GetUserManager<ApplicationUserManager>()
                    .FindById(userId);
                foreach (var course in courses)
                {
                    course.ApplicationUser = user;
                    var cate = context.Categories.FirstOrDefault(u => u.Id == course.CategoryId);
                    course.Category = cate;
                }

                return View(courses);
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            using (var context = new ApplicationDbContext())
            {
                var course = context.Courses.FirstOrDefault(u => u.LecturerId == userId && u.Id == id);
                if (course == null)
                    return HttpNotFound();
                ViewBag.Categories = context.Categories.ToList();
                return View(course);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Course course)
        {
            ModelState.Remove("LecturerId");
            ModelState.Remove("ApplicationUser");
            ModelState.Remove("Attendances");
            ModelState.Remove("Category");

            using (var context = new ApplicationDbContext())
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Categories = context.Categories.ToList();
                    return View("Edit", course);
                }

                //Update selected entity
                var result = context.Courses.Find(course.Id);
                result.LecturerId = User.Identity.GetUserId();
                result.Name = course.Name;
                result.Place = course.Place;
                result.CategoryId = course.CategoryId;
                result.Category = context.Categories.FirstOrDefault(u => u.Id == course.CategoryId);
                result.DateTime = course.DateTime;
                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName,
                            validationError.ErrorMessage);
                }

                return RedirectToAction("Mine", "Course");
            }
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            var userId = User.Identity.GetUserId();
            using (var context = new ApplicationDbContext())
            {
                var course = context.Courses.FirstOrDefault(u => (u.LecturerId == userId) & (u.Id == id));
                if (course == null)
                    return Json(false, JsonRequestBehavior.AllowGet);
                context.Attendances.RemoveRange(course.Attendances);
                context.Courses.Remove(course);
                context.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LectureIamGoing()
        {
            var currentUser =
                System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                    .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var context = new ApplicationDbContext();

            var listFollwee = context.Followings.Where(p => p.FollowerId == currentUser.Id).ToList();

            var listAttendances = context.Attendances.Where(p => p.AttendanceId == currentUser.Id).ToList();

            var courses = new List<Course>();
            foreach (var objCourse in from course in listAttendances
                from item in listFollwee
                where item.FolloweeId == course.Course.LecturerId
                select course.Course)
            {
                objCourse.ApplicationUser = System.Web.HttpContext.Current
                    .GetOwinContext()
                    .GetUserManager<ApplicationUserManager>()
                    .FindById(objCourse.LecturerId);
                courses.Add(objCourse);
            }

            return View(courses);
        }
    }
}