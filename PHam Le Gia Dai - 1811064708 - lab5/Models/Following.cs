namespace PHam_Le_Gia_Dai___1811064708___lab5.Models
{
    public class Following
    {
        public string FollowerId { get; set; }
        public virtual ApplicationUser Follower { get; set; }
        public string FolloweeId { get; set; }
        public virtual ApplicationUser Followee { get; set; }
    }
}