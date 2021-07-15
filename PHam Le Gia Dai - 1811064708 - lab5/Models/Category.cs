using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PHam_Le_Gia_Dai___1811064708___lab5.Models
{
    public class Category
    {
        public Category()
        {
            Courses = new HashSet<Course>();
        }

        public int Id { get; set; }

        [StringLength(120)] public string Name { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}