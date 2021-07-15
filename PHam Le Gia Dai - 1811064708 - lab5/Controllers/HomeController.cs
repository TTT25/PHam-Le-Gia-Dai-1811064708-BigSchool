using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PHam_Le_Gia_Dai___1811064708___lab5.Models;

namespace PHam_Le_Gia_Dai___1811064708___lab5.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var context = new ApplicationDbContext();
            var today = DateTime.UtcNow.AddHours(7);
            var newCourses = context.Courses.Where(u => u.DateTime > today)
                .OrderBy(u => u.DateTime).ToList();
            var result = new List<Course>();
            var attended = new List<string>();
            var followed = new List<string>();

            //Check if the user is Logged In.
            var userID = User.Identity.GetUserId();
            ViewBag.IsLoggedIn = !userID.IsEmpty();

            foreach (var course in newCourses)
            {
                var user = System.Web.HttpContext.Current
                    .GetOwinContext()
                    .GetUserManager<ApplicationUserManager>()
                    .FindById(course.LecturerId);

                //Check if the user has attend the course
                var find = context.Attendances.FirstOrDefault(p =>
                    p.CourseId == course.Id && p.AttendanceId == userID);
                if (find != null)
                    attended.Add(course.Name);

                //Check if the user has followed the lecturer
                var findFollow =
                    context.Followings.FirstOrDefault(p => p.FollowerId == userID && p.FolloweeId == course.LecturerId);
                if (findFollow != null)
                    followed.Add(findFollow.FolloweeId);

                course.ApplicationUser = user;
                course.Category = context.Categories.FirstOrDefault(u => u.Id == course.CategoryId);
                result.Add(course);
            }

            ViewBag.AttendedCourses = attended;
            ViewBag.FollowedIDs = followed;
            return View(result);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}