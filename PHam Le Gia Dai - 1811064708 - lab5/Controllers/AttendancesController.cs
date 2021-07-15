using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using PHam_Le_Gia_Dai___1811064708___lab5.Models;

namespace PHam_Le_Gia_Dai___1811064708___lab5.Controllers
{
    public class AttendancesController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Attend(Course attendanceDTO)
        {
            var userId = User.Identity.GetUserId();
            if (userId == null)
                return BadRequest("Authentication Credential not provided.");
            using (var context = new ApplicationDbContext())
            {
                var att = context.Attendances.FirstOrDefault(u =>
                    (u.AttendanceId == userId) & (u.CourseId == attendanceDTO.Id));
                if (att != null)
                {
                    context.Attendances.Remove(att);
                    context.SaveChanges();
                    return Ok("Canceled");
                }

                var atd = new Attendance
                {
                    AttendanceId = userId,
                    CourseId = attendanceDTO.Id
                };
                context.Attendances.Add(atd);
                context.SaveChanges();
            }

            return Ok();
        }
    }
}