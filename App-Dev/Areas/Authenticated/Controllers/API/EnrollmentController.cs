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
    public class EnrollmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        public EnrollmentController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allCategories = await _unitOfWork.Course.
                GetAllAsync(includeProperties: "CourseCategory");
            return Json(new { data = allCategories });
        }
        [HttpGet]
        public async Task<IActionResult> GetTrainee()
        {
            var allTraineeProfile = await _unitOfWork.TraineeProfile.GetAllAsync();
            return Json(new { data = allTraineeProfile });
        }
        [HttpPost]
        public async Task<IActionResult> Enrollment(int id, [FromBody] string traineeid)
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
                if (traineeid == null)
                {
                    return Json(new { success = false, message = "Traineeid null" });
                }

                var result = await _unitOfWork.Enrollment
                    .GetAllAsync(u => u.CourseId == id && u.TraineeId == traineeid);

                if (result.Any())
                {
                    return Json(new { success = false, message = "Already enroll" });
                }
                else
                {
                    Enrollment enrollment = new Enrollment()
                    {
                        CourseId = id,
                        TraineeId = traineeid,
                        Time = DateTime.Now,
                        EnrollStatus = SD.Approve
                    };
                    await _unitOfWork.Enrollment.AddAsync(enrollment);
                }
            }
            _unitOfWork.Save();
            return Json(new { success = true, message = "Operation Successful." });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, [FromBody] string traineeid)
        {
            var enrollment = await _unitOfWork.Enrollment
                .GetFirstOrDefaultAsync(u => u.CourseId == id && u.TraineeId == traineeid);
            if (enrollment == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }

            await _unitOfWork.Enrollment.RemoveAsync(enrollment);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }
    }
}
