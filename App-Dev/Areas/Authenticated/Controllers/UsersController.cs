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

            ForgotPasswordVM UserEmail = new ForgotPasswordVM()
            {
                Email = user.Email
            };
            return View(UserEmail);
        }
        [Authorize(Roles = SD.Role_Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
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
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
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
            UsersVM usersVm = new UsersVM();
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
        public async Task<IActionResult> Edit(UsersVM user)
        {
            UsersVM usersVm = new UsersVM();
            if (user.ApplicationUser != null)
            {
                usersVm.ApplicationUser = user.ApplicationUser;
                var userDb = await _unitOfWork.ApplicationUser.GetAllAsync(u => u.Id == user.ApplicationUser.Id);
                var isUserExists = userDb.Count() > 0 ? true : false;
                var userEmailDb = await _unitOfWork.ApplicationUser.GetAllAsync(u => u.Email == user.ApplicationUser.Email);
                var isEmailExists = userEmailDb.Count() > 0 ? true : false;
                if (!isEmailExists && !isUserExists)
                {
                    ViewData["Message"] = "Error: User with this email already exists";
                    return View(usersVm);
                }
        
                var userFromDb = await _unitOfWork.ApplicationUser.GetAsync(user.ApplicationUser.Id);
                userFromDb.Name= user.ApplicationUser.Name;
                userFromDb.Email = user.ApplicationUser.Email;

                await _unitOfWork.ApplicationUser.Update(userFromDb);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }else if (user.TraineeProfile != null)
            {
                usersVm.TraineeProfile = user.TraineeProfile;
                var userDb = await _unitOfWork.TraineeProfile.GetAllAsync(u => u.Id == user.TraineeProfile.Id);
                var isUserExists = userDb.Count() > 0 ? true : false;
                var userEmailDb = await _unitOfWork.TraineeProfile.GetAllAsync(u => u.Email == user.TraineeProfile.Email);
                var isEmailExists = userEmailDb.Count() > 0 ? true : false;
                if (!isEmailExists && !isUserExists)
                {
                    ViewData["Message"] = "Error: User with this email already exists";
                    return View(usersVm);
                }
        
                var userFromDb = await _unitOfWork.TraineeProfile.GetAsync(user.TraineeProfile.Id);
                userFromDb.Name= user.TraineeProfile.Name;
                userFromDb.Email = user.TraineeProfile.Email;
                userFromDb.DateOfBirth = user.TraineeProfile.DateOfBirth;
                userFromDb.Education = user.TraineeProfile.Education;
                userFromDb.MainProgrammingLanguage = user.TraineeProfile.MainProgrammingLanguage;
                userFromDb.ToeicScore = user.TraineeProfile.ToeicScore;
                userFromDb.ExperimentDetail = user.TraineeProfile.ExperimentDetail;
                userFromDb.Location = user.TraineeProfile.Location;
                userFromDb.Department = user.TraineeProfile.Department;
                await _unitOfWork.TraineeProfile.Update(userFromDb);
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
                var userDb = await _unitOfWork.TrainerProfile.GetAllAsync(u => u.Id == user.TrainerProfile.Id);
                var isUserExists = userDb.Count() > 0 ? true : false;
                var userEmailDb = await _unitOfWork.TrainerProfile.GetAllAsync(u => u.Email == user.TrainerProfile.Email);
                var isEmailExists = userEmailDb.Count() > 0 ? true : false;
                if (!isEmailExists && !isUserExists)
                {
                    ViewData["Message"] = "Error: User with this email already exists";
                    return View(usersVm);
                }
        
                var userFromDb = await _unitOfWork.TrainerProfile.GetAsync(user.TrainerProfile.Id);
                userFromDb.Name= user.TrainerProfile.Name;
                userFromDb.Email = user.TrainerProfile.Email;
                userFromDb.WorkingPlace = user.TrainerProfile.WorkingPlace;
                userFromDb.TypeOfTrainer = user.TrainerProfile.TypeOfTrainer;

                await _unitOfWork.TrainerProfile.Update(userFromDb);
                _unitOfWork.Save();
                if (User.IsInRole(SD.Role_Staff))
                {
                    return RedirectToAction(nameof(TrainerManager));
                }
                return RedirectToAction(nameof(Index));
            }
               
        }
        
        #region Api Calls

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (User.IsInRole(SD.Role_Admin))
            {
                var claimsIdentity = (ClaimsIdentity) User.Identity;
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var userList = await _unitOfWork.ApplicationUser.GetAllAsync(u=> u.Id != claims.Value);

                foreach (var user in userList)
                {
                    var usertemp = await _userManager.FindByIdAsync(user.Id);
                    var roleTemp = await _userManager.GetRolesAsync(usertemp);
                    user.Role = roleTemp.FirstOrDefault();
                }

                var userListTemp = userList.Where(u => u.Role != SD.Role_Trainee); 
                return Json(new {data = userListTemp});
            }
            var traineeUserTemp = await _unitOfWork.ApplicationUser.GetAllAsync();
            foreach (var user in traineeUserTemp)
            {
                var usertemp = await _userManager.FindByIdAsync(user.Id);
                var roleTemp = await _userManager.GetRolesAsync(usertemp);
                user.Role = roleTemp.FirstOrDefault();
            }

            var traineeUser = traineeUserTemp.Where(u => u.Role == SD.Role_Trainee || u.Role == SD.Role_Trainer);
            return Json(new {data = traineeUser});
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTrainer()
        {
            var claimsIdentity = (ClaimsIdentity) User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userList = await _unitOfWork.ApplicationUser.GetAllAsync(u=> u.Id != claims.Value);

            foreach (var user in userList)
            {
                var usertemp = await _userManager.FindByIdAsync(user.Id);
                var roleTemp = await _userManager.GetRolesAsync(usertemp);
                user.Role = roleTemp.FirstOrDefault();
            }

            var userListTemp = userList.Where(u => u.Role == SD.Role_Trainer); 
            return Json(new {data = userListTemp});
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTrainee()
        {
            var claimsIdentity = (ClaimsIdentity) User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userList = await _unitOfWork.ApplicationUser.GetAllAsync(u=> u.Id != claims.Value);

            foreach (var user in userList)
            {
                var usertemp = await _userManager.FindByIdAsync(user.Id);
                var roleTemp = await _userManager.GetRolesAsync(usertemp);
                user.Role = roleTemp.FirstOrDefault();
            }

            var userListTemp = userList.Where(u => u.Role == SD.Role_Trainee); 
            return Json(new {data = userListTemp});
        }
        [HttpPost]
        public async Task<IActionResult> LockUnlock([FromBody] string id)
        {
            var claimsIdentity = (ClaimsIdentity) User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userFromDb = await _unitOfWork.ApplicationUser.GetFirstOrDefaultAsync(u => u.Id == claims.Value);
            
            var objFromDb = await _unitOfWork.ApplicationUser.GetFirstOrDefaultAsync(u => u.Id == id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }
            if (userFromDb.Id == objFromDb.Id)
            {
                return Json(new { success = false, message = "Error You are currently lock your account" });
            }
            if(objFromDb.LockoutEnd!=null && objFromDb.LockoutEnd > DateTime.Now)
            {
                //user is currently locked, we will unlock them
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            _unitOfWork.Save();
            return Json(new { success = true, message = "Operation Successful." });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var objFromDb = await _unitOfWork.ApplicationUser.GetAsync(id);
            if (objFromDb == null)
            {
                return Json(new {success = false, message = "Error while Deleting"});
            }
            await _userManager.DeleteAsync(objFromDb);
            return Json(new {success = true, message = "Delete successful"});
        }
        #endregion

    }
}