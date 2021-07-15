using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using PHam_Le_Gia_Dai___1811064708___lab5.Models;

namespace PHam_Le_Gia_Dai___1811064708___lab5.Controllers
{
    public class FollowController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Follow(Following follow)
        {
            var curr = User.Identity.GetUserId();
            if ((curr == null) | curr.Equals(string.Empty))
                return BadRequest("Credential Information not provided.");
            if (curr == follow.FolloweeId)
                return BadRequest("You can not follow your self.");
            using (var context = new ApplicationDbContext())
            {
                var isFollowed =
                    context.Followings.FirstOrDefault(u =>
                        u.FolloweeId.Equals(follow.FolloweeId) & u.FollowerId.Equals(curr));
                if (isFollowed != null)
                {
                    context.Followings.Remove(isFollowed);
                    context.SaveChanges();
                    return Ok("UnFollowed");
                }

                follow.FollowerId = curr;
                context.Followings.Add(follow);
                context.SaveChanges();
                return Ok("Following");
            }
        }
    }
}