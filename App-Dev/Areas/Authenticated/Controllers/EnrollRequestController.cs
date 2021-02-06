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
        public IActionResult Index()
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
            var isExist = await _unitOfWork.Enrollment
                .GetAllAsync(u => u.CourseId == id && u.TraineeId == claims.Value && u.EnrollStatus == SD.Request);
                
            if (isExist.Count() == 0)
            {
                Enrollment enrollment = new Enrollment()
                {
                    CourseId = id,
                    TraineeId = claims.Value,
                    Time = DateTime.Now,
                    EnrollStatus = SD.Request
                };
                await _unitOfWork.Enrollment.AddAsync(enrollment);
                _unitOfWork.Save();
            }
            else
            {
                TempData["Message"] = "Error: Your request already send";
                return RedirectToAction("AvailableCourse", "Trainee");
            }

            return View("ConfirmRequest");
        }
    }
}