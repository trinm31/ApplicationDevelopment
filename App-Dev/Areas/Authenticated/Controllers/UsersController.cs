using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using App_Dev.DataAccess.Repository.IRepository;
using App_Dev.Models;
using App_Dev.Models.ViewModels;
using App_Dev.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App_Dev.Areas.Authenticated.Controllers
{
    [Area("Authenticated")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Staff)]
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        

        public UsersController( IUnitOfWork unitOfWork,UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        // GET
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult TraineeManager()
        {
            return View();
        }
        public IActionResult TrainerManager()
        {
            return View();
        }
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> ForgotPassword(string id)
        {
            var user = await _unitOfWork.ApplicationUser.GetAsync(id);

            if (user == null)
            {
                return View();
            }

            ForgotPasswordViewModel UserEmail = new ForgotPasswordViewModel()
            {
                Email = user.Email
            };
            return View(UserEmail);
        }
        [Authorize(Roles = SD.Role_Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    return RedirectToAction("ResetPassword", "Users", new {email = model.Email, token = token});
                }

                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                ModelState.AddModelError("","Invalid password reset token");
            }

            return View();
        }
        [Authorize(Roles = SD.Role_Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user,model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        return View("ResetPasswordConfirmation");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("",error.Description);
                    }

                    return View(model);
                }
            }
            return View(model);
        }
        [Authorize(Roles = SD.Role_Staff)]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _unitOfWork.ApplicationUser.GetAsync(id);

            if (user == null)
            {
                return View();
            }
            var usertemp = await _userManager.FindByIdAsync(user.Id);
            var roleTemp = await _userManager.GetRolesAsync(usertemp);
            user.Role = roleTemp.FirstOrDefault();
            UsersViewModel usersVm = new UsersViewModel();
            if (user.Role == SD.Role_Trainer)
            {
                var trainerProfile = await _unitOfWork.TrainerProfile.GetAsync(id);
                usersVm.TrainerProfile = trainerProfile;
            }
            else if (user.Role == SD.Role_Trainee)
            {
                var traineeProfile = await _unitOfWork.TraineeProfile.GetAsync(id);
                usersVm.TraineeProfile = traineeProfile;
            }
            else
            {
                usersVm.ApplicationUser = user;
            }
            return View(usersVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.Role_Staff)]
        public async Task<IActionResult> Edit(UsersViewModel user)
        {
            UsersViewModel usersVm = new UsersViewModel();
            if (user.ApplicationUser != null)
            {
                usersVm.ApplicationUser = user.ApplicationUser;
                var applicationUsers = await _unitOfWork.ApplicationUser.GetAllAsync(u => u.Id == user.ApplicationUser.Id);
                var userEmailDb = await _unitOfWork.ApplicationUser.GetAllAsync(u => u.Email == user.ApplicationUser.Email);
                if (!userEmailDb.Any() && !applicationUsers.Any())
                {
                    ViewData["Message"] = "Error: User with this email already exists";
                    return View(usersVm);
                }
        
                var applicationUser = await _unitOfWork.ApplicationUser.GetAsync(user.ApplicationUser.Id);
                applicationUser.Name= user.ApplicationUser.Name;
                applicationUser.Email = user.ApplicationUser.Email;

                await _unitOfWork.ApplicationUser.Update(applicationUser);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }else if (user.TraineeProfile != null)
            {
                usersVm.TraineeProfile = user.TraineeProfile;
                var traineeProfiles = await _unitOfWork.TraineeProfile.GetAllAsync(u => u.Id == user.TraineeProfile.Id);
                var userEmailDb = await _unitOfWork.TraineeProfile.GetAllAsync(u => u.Email == user.TraineeProfile.Email);
                var traineeProfile = await _unitOfWork.TraineeProfile.GetAsync(user.TraineeProfile.Id);
                if (!userEmailDb.Any() && !traineeProfiles.Any())
                {
                    ViewData["Message"] = "Error: User with this email already exists";
                    return View(usersVm);
                }
                traineeProfile.Name= user.TraineeProfile.Name;
                traineeProfile.Email = user.TraineeProfile.Email;
                traineeProfile.DateOfBirth = user.TraineeProfile.DateOfBirth;
                traineeProfile.Education = user.TraineeProfile.Education;
                traineeProfile.MainProgrammingLanguage = user.TraineeProfile.MainProgrammingLanguage;
                traineeProfile.ToeicScore = user.TraineeProfile.ToeicScore;
                traineeProfile.ExperimentDetail = user.TraineeProfile.ExperimentDetail;
                traineeProfile.Location = user.TraineeProfile.Location;
                traineeProfile.Department = user.TraineeProfile.Department;
                traineeProfile.PhoneNumber = user.TraineeProfile.PhoneNumber;
                await _unitOfWork.TraineeProfile.Update(traineeProfile);
                _unitOfWork.Save();
                if (User.IsInRole(SD.Role_Staff))
                {
                    return RedirectToAction(nameof(TraineeManager));
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                usersVm.TrainerProfile = user.TrainerProfile;
                var trainerProfiles = await _unitOfWork.TrainerProfile.GetAllAsync(u => u.Id == user.TrainerProfile.Id);
                var userEmailDb = await _unitOfWork.TrainerProfile.GetAllAsync(u => u.Email == user.TrainerProfile.Email);
                var trainerProfile = await _unitOfWork.TrainerProfile.GetAsync(user.TrainerProfile.Id);
                if (!userEmailDb.Any() && !trainerProfiles.Any())
                {
                    ViewData["Message"] = "Error: User with this email already exists";
                    return View(usersVm);
                }


                trainerProfile.Name= user.TrainerProfile.Name;
                trainerProfile.Email = user.TrainerProfile.Email;
                trainerProfile.WorkingPlace = user.TrainerProfile.WorkingPlace;
                trainerProfile.TypeOfTrainer = user.TrainerProfile.TypeOfTrainer;
                trainerProfile.PhoneNumber = user.TrainerProfile.PhoneNumber;

                await _unitOfWork.TrainerProfile.Update(trainerProfile);
                _unitOfWork.Save();
                if (User.IsInRole(SD.Role_Staff))
                {
                    return RedirectToAction(nameof(TrainerManager));
                }
                return RedirectToAction(nameof(Index));
            }       
        }
    }
}