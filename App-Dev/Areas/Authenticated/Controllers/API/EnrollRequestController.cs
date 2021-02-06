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
using System.Threading.Tasks;

namespace App_Dev.Areas.Authenticated.Controllers.Api
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
        public async Task<IActionResult> GetEnrollList(string status)
        {
            IEnumerable<Enrollment> requestList = new List<Enrollment>();

            if (User.IsInRole(SD.Role_Staff))
            {
                requestList = await _unitOfWork.Enrollment.GetAllAsync(includeProperties: "TraineeProfile,Course");
                switch (status)
                {
                    case "inprocess":
                        requestList = requestList.Where(o => o.EnrollStatus == SD.Request);
                        break;
                    case "approve":
                        requestList = requestList.Where(o => o.EnrollStatus == SD.Approve);
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
                Enrollment enroll = await _unitOfWork.Enrollment.GetFirstOrDefaultAsync(u => u.Id == idInt);
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
                Enrollment enroll = await _unitOfWork.Enrollment.GetFirstOrDefaultAsync(u => u.Id == idInt);
                if (enroll == null)
                {
                    return Json(new { success = false, message = "Can not find request" });
                }
                enroll.EnrollStatus = SD.Reject;
                _unitOfWork.Save();
            }
            return Json(new { success = true, message = "Operation Successful." });
        }
    }
}
