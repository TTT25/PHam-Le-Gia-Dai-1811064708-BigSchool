using System.Collections.Generic;
using System.Data.Entity.Migrations;
using PHam_Le_Gia_Dai___1811064708___lab5.Models;

namespace PHam_Le_Gia_Dai___1811064708___lab5.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }


        protected override void Seed(ApplicationDbContext context)
        {
            ICollection<Category> categories = new List<Category>
            {
                new Category
                {
                    Name = "Math"
                },
                new Category
                {
                    Name = "Physic"
                },
                new Category
                {
                    Name = "English"
                },
                new Category
                {
                    Name = "iT"
                }
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();
        }
    }
}