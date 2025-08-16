using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SchoolManagementSystem.DataAccess
{
    public class SchoolDbContext:IdentityDbContext<AppUser>
    {
       public SchoolDbContext(DbContextOptions<SchoolDbContext> options):base(options)
        {
        }

       
        public DbSet<Student> Students { get; set; }
         public DbSet<Teacher> Teachers { get; set; }
         public DbSet<Class> Classes { get; set; }
         public DbSet<Course> Courses { get; set; }
         public DbSet<StudentCourse> StudentCourses { get; set; }


         protected override void OnModelCreating(ModelBuilder builder)
         {
            base.OnModelCreating(builder);

            foreach (var relationship in builder.Model
                .GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
           

        }

    }
}
