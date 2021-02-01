using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using App_Dev.DataAccess.Repository.IRepository;
using App_Dev.Models;
using App_Dev.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace App_Dev.Areas.Authenticated.Controllers
{
    [Area("Authenticated")]
    [Authorize]
    public class EnrollRequestController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        public EnrollRequestController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }
        
        
        [HttpGet]
        [Authorize(Roles = SD.Role_Trainee)]
        public async Task<IActionResult> RequestEnroll(int id)
        {
            var coursefromDb = await _unitOfWork.Course.GetAsync(id);
            if (coursefromDb == null)
            {
                return NotFound();
            }
            var claimsIdentity = (ClaimsIdentity) User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var isExist = await _unitOfWork.Enroll.GetAllAsync(u => u.CourseId == id && u.TraineeId == claims.Value && u.EnrollStatus == SD.Request);
                
            if (isExist.Count() == 0)
            {
                Enroll enroll = new Enroll()
                {
                    CourseId = id,
                    TraineeId = claims.Value,
                    Time = DateTime.Now,
                    EnrollStatus = SD.Request
                };
                await _unitOfWork.Enroll.AddAsync(enroll);
                _unitOfWork.Save();
            }
            else
            {
                TempData["Message"] = "Error: Your request already send";
                return RedirectToAction("AvailableCourse", "Trainee");
            }

            return View("ConfirmRequest");
        }
        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetEnrollList(string status)
        {
            IEnumerable<Enroll> requestList = new List<Enroll>();

            if (User.IsInRole(SD.Role_Staff))
            {
                requestList = await _unitOfWork.Enroll.GetAllAsync(includeProperties: "TraineeProfile,Course");
                switch (status)
                {
                    case "inprocess":
                        requestList = requestList.Where(o => o.EnrollStatus==SD.Request);
                        break;
                    case "approve":
                        requestList = requestList.Where(o =>  o.EnrollStatus==SD.Approve);
                        break;
                    case "rejected":
                        requestList = requestList.Where(o => o.EnrollStatus == SD.Reject);
                        break;
                    default:
                        break;
                }
            }
            return Json(new { data = requestList });
        }
        
        [HttpPost]
        public async Task<IActionResult> Approve([FromBody] string id)
        {
            var idInt = Convert.ToInt32(id);
            if (User.IsInRole(SD.Role_Staff))
            {
                Enroll enroll = await _unitOfWork.Enroll.GetFirstOrDefaultAsync(u => u.Id == idInt);
                if (enroll == null)
                {
                    return Json(new { success = false, message = "Can not find request" });
                }
                enroll.EnrollStatus = SD.Approve;
                _unitOfWork.Save();
                
            }
            return Json(new { success = true, message = "Operation Successful." });
        }
        
        [HttpPost]
        public async Task<IActionResult> Reject([FromBody] string id)
        {
            var idInt = Convert.ToInt32(id);
            if (User.IsInRole(SD.Role_Staff))
            {
                Enroll enroll = await _unitOfWork.Enroll.GetFirstOrDefaultAsync(u => u.Id == idInt);
                if (enroll == null)
                {
                    return Json(new { success = false, message = "Can not find request" });
                }
                enroll.EnrollStatus = SD.Reject;
                _unitOfWork.Save();
            }
            return Json(new { success = true, message = "Operation Successful." });
        }

        #endregion
    }
}