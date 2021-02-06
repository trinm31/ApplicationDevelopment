using System;
using System.Collections.Generic;
using System.Text;
using App_Dev.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App_Dev.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseTrainer> courseTrainers { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }
        public DbSet<Enrollment> Enrolls { get; set; }
        public DbSet<TraineeProfile> TraineeProfiles { get; set; }
        public DbSet<TrainerProfile> TrainerProfiles { get; set; }
        
    }
}