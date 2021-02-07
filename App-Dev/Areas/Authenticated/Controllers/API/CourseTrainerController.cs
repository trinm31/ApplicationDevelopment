using App_Dev.DataAccess.Repository.IRepository;
using App_Dev.Models;
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

namespace App_Dev.Areas.Authenticated.Controllers.Api
{
    [Area("Authenticated")]
    [Authorize(Roles = SD.Role_Staff)]
    public class CourseTrainerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        public CourseTrainerController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allcategories = await _unitOfWork.Course
                .GetAllAsync(includeProperties: "CourseCategory");
            return Json(new { data = allcategories });
        }
        [HttpGet]
        public async Task<IActionResult> GetTrainer()
        {
            var alltrainerprofiles = await _unitOfWork.TrainerProfile.GetAllAsync();
            return Json(new { data = alltrainerprofiles });
        }
        [HttpPost]
        public async Task<IActionResult> CourseTrainer(int id, [FromBody] string trainerid)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var currentUser = await _userManager.FindByIdAsync(claims.Value);
            var currentUserRole = await _userManager.GetRolesAsync(currentUser);
            if (currentUserRole.FirstOrDefault() == SD.Role_Staff)
            {
                if (id == 0)
                {
                    return Json(new { success = false, message = "Course id null" });
                }
                if (trainerid == null)
                {
                    return Json(new { success = false, message = "Trainerid null" });
                }
                var result = await _unitOfWork.CourseTrainer
                    .GetAllAsync(u => u.CourseId == id && u.TrainerId == trainerid);
                if (result.Any())
                {
                    return Json(new { success = false, message = "Already Assign" });
                }
                else
                {
                    CourseTrainer courseTrainer = new CourseTrainer()
                    {
                        CourseId = id,
                        TrainerId = trainerid,
                        Time = DateTime.Now
                    };
                    await _unitOfWork.CourseTrainer.AddAsync(courseTrainer);
                }
                var maxTrainer = await _unitOfWork.CourseTrainer
                    .GetAllAsync(u => u.CourseId == id);
                if (maxTrainer.Count() >= SD.MaxTrainerNumber)
                {
                    return Json(new
                    {
                        success = false,
                        message = $"Already {SD.MaxTrainerNumber} trainer has assigned"
                    });
                }
            }
            _unitOfWork.Save();
            return Json(new { success = true, message = "Operation Successful." });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, [FromBody] string trainerid)
        {
            var getCourseTrainer = await _unitOfWork.CourseTrainer
                .GetFirstOrDefaultAsync(u => u.CourseId == id && u.TrainerId == trainerid);
            if (getCourseTrainer == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            await _unitOfWork.CourseTrainer.RemoveAsync(getCourseTrainer);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }
    }
}
