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
            var allObj = await _unitOfWork.Course
                .GetAllAsync(includeProperties: "CourseCategory");
            return Json(new { data = allObj });
        }
        [HttpGet]
        public async Task<IActionResult> GetTrainer()
        {
            var allObj = await _unitOfWork.TrainerProfile.GetAllAsync();
            return Json(new { data = allObj });
        }
        [HttpPost]
        public async Task<IActionResult> CourseTrainer(int id, [FromBody] string trainerid)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userFromDb = await _unitOfWork.ApplicationUser
                .GetFirstOrDefaultAsync(u => u.Id == claims.Value);
            var usertemp = await _userManager.FindByIdAsync(userFromDb.Id);
            var roleTemp = await _userManager.GetRolesAsync(usertemp);
            userFromDb.Role = roleTemp.FirstOrDefault();
            if (roleTemp.FirstOrDefault() == SD.Role_Staff)
            {
                if (id == null)
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
                    CourseTrainer courseTrainer = new CourseTrainer()
                    {
                        CourseId = id,
                        TrainerId = trainerid,
                        Time = DateTime.Now
                    };
                    await _unitOfWork.CourseTrainer.AddAsync(courseTrainer);
                }
                else
                {
                    return Json(new { success = false, message = "Already Assign" });
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
            var objFromDb = await _unitOfWork.CourseTrainer
                .GetFirstOrDefaultAsync(u => u.CourseId == id && u.TrainerId == trainerid);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            await _unitOfWork.CourseTrainer.RemoveAsync(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }
    }
}
