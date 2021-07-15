namespace PHam_Le_Gia_Dai___1811064708___lab5.Models
{
    public class Attendance
    {
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
        public string AttendanceId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}