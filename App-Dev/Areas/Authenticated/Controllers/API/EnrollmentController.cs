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
            var allObj = await _unitOfWork.Course.GetAllAsync(includeProperties: "CourseCategory");
            return Json(new { data = allObj });
        }
        [HttpGet]
        public async Task<IActionResult> GetTrainee()
        {
            var allObj = await _unitOfWork.TraineeProfile.GetAllAsync();
            return Json(new { data = allObj });
        }
        [HttpPost]
        public async Task<IActionResult> Enrollment(int id, [FromBody] string traineeid)
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
                if (traineeid == null)
                {
                    return Json(new { success = false, message = "Traineeid null" });
                }

                var isExist = await _unitOfWork.Enrollment
                    .GetAllAsync(u => u.CourseId == id && u.TraineeId == traineeid);

                if (isExist.Count() == 0)
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
                else
                {
                    return Json(new { success = false, message = "Already enroll" });
                }
            }
            _unitOfWork.Save();
            return Json(new { success = true, message = "Operation Successful." });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, [FromBody] string traineeid)
        {
            var objFromDb = await _unitOfWork.Enrollment
                .GetFirstOrDefaultAsync(u => u.CourseId == id && u.TraineeId == traineeid);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }

            await _unitOfWork.Enrollment.RemoveAsync(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }
    }
}
