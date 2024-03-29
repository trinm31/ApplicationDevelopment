﻿using App_Dev.DataAccess.Repository.IRepository;
using App_Dev.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace App_Dev.Areas.Authenticated.Controllers.API
{
    [Area("Authenticated")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Staff)]
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public UsersController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (User.IsInRole(SD.Role_Admin))
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var userList = await _unitOfWork.ApplicationUser.GetAllAsync(u => u.Id != claims.Value);
                foreach (var user in userList)
                {
                    var usertemp = await _userManager.FindByIdAsync(user.Id);
                    var roleTemp = await _userManager.GetRolesAsync(usertemp);
                    user.Role = roleTemp.FirstOrDefault();
                }

                var userListTemp = userList.Where(u => u.Role != SD.Role_Trainee);
                return Json(new { data = userListTemp });
            }
            var traineeUserTemp = await _unitOfWork.ApplicationUser.GetAllAsync();
            foreach (var user in traineeUserTemp)
            {
                var usertemp = await _userManager.FindByIdAsync(user.Id);
                var roleTemp = await _userManager.GetRolesAsync(usertemp);
                user.Role = roleTemp.FirstOrDefault();
            }

            var traineeUser = traineeUserTemp.Where(u => u.Role == SD.Role_Trainee || u.Role == SD.Role_Trainer);
            return Json(new { data = traineeUser });
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTrainer()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userList = await _unitOfWork.ApplicationUser.GetAllAsync(u => u.Id != claims.Value);

            foreach (var user in userList)
            {
                var usertemp = await _userManager.FindByIdAsync(user.Id);
                var roleTemp = await _userManager.GetRolesAsync(usertemp);
                user.Role = roleTemp.FirstOrDefault();
            }

            var userListTemp = userList.Where(u => u.Role == SD.Role_Trainer);
            return Json(new { data = userListTemp });
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTrainee()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userList = await _unitOfWork.ApplicationUser.GetAllAsync(u => u.Id != claims.Value);

            foreach (var user in userList)
            {
                var usertemp = await _userManager.FindByIdAsync(user.Id);
                var roleTemp = await _userManager.GetRolesAsync(usertemp);
                user.Role = roleTemp.FirstOrDefault();
            }

            var userListTemp = userList.Where(u => u.Role == SD.Role_Trainee);
            return Json(new { data = userListTemp });
        }
        
        [HttpPost]
        // httpget
        public async Task<IActionResult> LockUnlock([FromBody] string id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var claimUser = await _unitOfWork.ApplicationUser.GetFirstOrDefaultAsync(u => u.Id == claims.Value);

            var applicationUser = await _unitOfWork.ApplicationUser.GetFirstOrDefaultAsync(u => u.Id == id);
            if (applicationUser == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
                //return notfound;
            }
            if (claimUser.Id == applicationUser.Id)
            {
                return Json(new { success = false, message = "Error You are currently lock your account" });
                //return notfound;
            }
            if (applicationUser.LockoutEnd != null && applicationUser.LockoutEnd > DateTime.Now)
            {
                //user is currently locked, we will unlock them
                applicationUser.LockoutEnd = DateTime.Now;
            }
            else
            {
                applicationUser.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            _unitOfWork.Save();
            //db.savechange();
            return Json(new { success = true, message = "Operation Successful." });
            //Return View(nameof(Index))
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var applicationUser = await _unitOfWork.ApplicationUser.GetAsync(id);
            if (applicationUser == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            await _userManager.DeleteAsync(applicationUser);
            return Json(new { success = true, message = "Delete successful" });
        }
    }
}
