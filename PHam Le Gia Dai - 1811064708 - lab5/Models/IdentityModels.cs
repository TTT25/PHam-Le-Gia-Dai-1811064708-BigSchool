using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PHam_Le_Gia_Dai___1811064708___lab5.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "This field is required.")]
        [StringLength(100, ErrorMessage = "This field can not be too long.")]
        public string Name { get; set; }

        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<Following> Followers { get; set; }
        public virtual ICollection<Following> Followees { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", false)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<Following> Followings { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Category>().HasKey(u => u.Id);
            modelBuilder.Entity<Category>().Property(u => u.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Category>()
                .HasMany(e => e.Courses)
                .WithRequired(e => e.Category)
                .HasForeignKey(e => e.CategoryId);

            modelBuilder.Entity<Course>().HasKey(u => u.Id);
            modelBuilder.Entity<Course>().Property(u => u.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Course>()
                .HasOptional(e => e.ApplicationUser)
                .WithMany()
                .HasForeignKey(u => u.LecturerId);

            modelBuilder.Entity<Course>()
                .HasMany(u => u.Attendances)
                .WithRequired(u => u.Course)
                .HasForeignKey(u => u.CourseId);
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Attendances)
                .WithRequired(u => u.ApplicationUser)
                .HasForeignKey(u => u.AttendanceId);
            modelBuilder.Entity<Attendance>().HasKey(u => new {u.CourseId, u.AttendanceId});

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Followees)
                .WithRequired(u => u.Followee)
                .HasForeignKey(u => u.FolloweeId);
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Followers)
                .WithRequired(u => u.Follower)
                .HasForeignKey(u => u.FollowerId);
            modelBuilder.Entity<Following>().HasKey(u => new {u.FolloweeId, u.FollowerId});
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}