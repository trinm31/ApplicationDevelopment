using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using App_Dev.DataAccess.Repository;
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
    [Authorize(Roles = SD.Role_Trainee)]
    public class TraineeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        public TraineeController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var objFromdb = await _unitOfWork.Enroll.GetAllAsync(u => u.TraineeId == claims.Value && u.EnrollStatus == SD.Approve);
            List<Course> courses = new List<Course>();
            foreach (var obj in objFromdb)
            {
                var course = await _unitOfWork.Course.GetFirstOrDefaultAsync(c => c.Id == obj.CourseId, includeProperties: "CourseCategory");
                courses.Add(course);
            }
            return View(courses);
        }
        public async Task<IActionResult> AvailableCourse()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var objFromdb = await _unitOfWork.Enroll.GetAllAsync(u => u.TraineeId == claims.Value && u.EnrollStatus == SD.Approve);
            List<Course> availablecourse = new List<Course>();
            var allcourse = await _unitOfWork.Course.GetAllAsync(includeProperties:"CourseCategory");
            availablecourse = allcourse.Except(allcourse.Where(i => objFromdb.Select(o => o.CourseId).ToList().Contains(i.Id))).ToList();
            ViewData["Message"] = TempData["Message"];
            return View(availablecourse);
        }
    }
}
