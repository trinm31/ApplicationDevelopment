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
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace App_Dev.Areas.Authenticated.Controllers
{
    [Area("Authenticated")]
    [Authorize(Roles = SD.Role_Staff)]
    public class AssignToTrainerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        public AssignToTrainerController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        // GET
        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> AssignToTrainer()
        {
            var trainerList = await _unitOfWork.TrainerProfile.GetAllAsync();
            var assignList = await _unitOfWork.CourseAssignToTrainer.GetAllAsync();
            AssignToTrainerVM assignToTrainerVm = new AssignToTrainerVM()
            {
                TrainerList = trainerList,
                AssignList = assignList
            };
            return View(assignToTrainerVm);
        }
        
        #region Api Calls

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allObj = await _unitOfWork.Course.GetAllAsync(includeProperties:"CourseCategory");
            return Json(new {data = allObj});
        }
        [HttpGet]
        public async Task<IActionResult> GetTrainer()
        {
            var allObj = await _unitOfWork.TrainerProfile.GetAllAsync();
            return Json(new {data = allObj});
        }
        [HttpPost]
        public async Task<IActionResult> AssignToTrainer(int id, [FromBody] string trainerid)
        {
            var claimsIdentity = (ClaimsIdentity) User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userFromDb = await _unitOfWork.ApplicationUser.GetFirstOrDefaultAsync(u => u.Id == claims.Value);
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
               
                
                var result = await _unitOfWork.CourseAssignToTrainer
                    .GetAllAsync(u => u.CourseId == id && u.TrainerId == trainerid);
                
                if (result.Any())
                {
                    CourseAssignToTrainer courseAssignToTrainer = new CourseAssignToTrainer()
                    {
                        CourseId = id,
                        TrainerId = trainerid,
                        Time = DateTime.Now
                    };
                    await _unitOfWork.CourseAssignToTrainer.AddAsync(courseAssignToTrainer);
                }
                else
                {
                    return Json(new { success = false, message = "Already Assign" });
                }
                var maxTrainer = await _unitOfWork.CourseAssignToTrainer
                    .GetAllAsync(u => u.CourseId == id);
                if (maxTrainer.Count() >= SD.MaxTrainerNumberDisplay) // magic number, magic string 
                {
                    return Json(new
                    {
                        success = false, 
                        message = $"Already {SD.MaxTrainerNumberDisplay} trainer has assigned"
                    });
                }
            }
            _unitOfWork.Save();
            return Json(new { success = true, message = "Operation Successful." });
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(int id, [FromBody] string trainerid)
        {
            var objFromDb = await _unitOfWork.CourseAssignToTrainer
                .GetFirstOrDefaultAsync(u => u.CourseId == id && u.TrainerId == trainerid);
            if (objFromDb == null)
            {
                return Json(new {success = false, message = "Error while Deleting"});
            }
            await _unitOfWork.CourseAssignToTrainer.RemoveAsync(objFromDb);
            _unitOfWork.Save();
            return Json(new {success = true, message = "Delete successful"});
        }
        #endregion
    }
}