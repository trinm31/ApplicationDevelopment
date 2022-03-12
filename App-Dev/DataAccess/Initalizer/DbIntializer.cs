using System;
using System.Linq;
using App_Dev.DataAccess.Data;
using App_Dev.Models;
using App_Dev.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace App_Dev.DataAccess.Initalizer
{
    public class DbIntializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        

        public DbIntializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch(Exception ex)
            {
                
            }

            if (_db.Roles.Any(r => r.Name == SD.Role_Admin)) return;
            if (_db.Roles.Any(r => r.Name == SD.Role_Staff)) return;
            if (_db.Roles.Any(r => r.Name == SD.Role_Trainee)) return;
            if (_db.Roles.Any(r => r.Name == SD.Role_Trainer)) return;

            _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Staff)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Trainee)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Trainer)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                Name = "Admin"
            },"Admin123@").GetAwaiter().GetResult() ;

            ApplicationUser userAdmin = _db.ApplicationUsers.Where(u => u.Email == "admin@gmail.com").FirstOrDefault();

            _userManager.AddToRoleAsync(userAdmin, SD.Role_Admin).GetAwaiter().GetResult();
            
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "Staff@gmail.com",
                Email = "Staff@gmail.com",
                EmailConfirmed = true,
                Name = "Staff"
            },"Staff123@").GetAwaiter().GetResult() ;

            ApplicationUser userStaff = _db.ApplicationUsers.Where(u => u.Email == "Staff@gmail.com").FirstOrDefault();

            _userManager.AddToRoleAsync(userStaff, SD.Role_Staff).GetAwaiter().GetResult();
            _userManager.CreateAsync(new TraineeProfile()
            {
                UserName = "Trainee@gmail.com",
                Email = "Trainee@gmail.com",
                EmailConfirmed = true,
                Name = "Trainee"
            },"Trainee123@").GetAwaiter().GetResult() ;

            TraineeProfile Trainee = _db.TraineeProfiles.Where(u => u.Email == "Trainee@gmail.com").FirstOrDefault();

            _userManager.AddToRoleAsync(Trainee, SD.Role_Trainee).GetAwaiter().GetResult();
            _userManager.CreateAsync(new TrainerProfile()
            {
                UserName = "Trainer@gmail.com",
                Email = "Trainer@gmail.com",
                EmailConfirmed = true,
                Name = "Trainer"
            },"Trainer123@").GetAwaiter().GetResult() ;

            TrainerProfile trainer = _db.TrainerProfiles.Where(u => u.Email == "Trainer@gmail.com").FirstOrDefault();

            _userManager.AddToRoleAsync(trainer, SD.Role_Trainer).GetAwaiter().GetResult();
        }
    }
}