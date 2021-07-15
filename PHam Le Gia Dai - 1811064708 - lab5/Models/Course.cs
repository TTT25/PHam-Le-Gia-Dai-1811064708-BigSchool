using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PHam_Le_Gia_Dai___1811064708___lab5.Models
{
    public class Course
    {
        public Course()
        {
            //Attendances = new List<Attendance>();
            DateTime = DateTime.UtcNow.AddHours(7);
        }

        public int Id { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "This field is required.")]
        public string Place { get; set; }

        [StringLength(120)]
        [Required(ErrorMessage = "This field is required.")]
        public string Name { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateTime { get; set; }

        [StringLength(128)] public string LecturerId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public virtual Category Category { get; set; }

        public virtual ICollection<Attendance> Attendances { get; set; }
    }
}