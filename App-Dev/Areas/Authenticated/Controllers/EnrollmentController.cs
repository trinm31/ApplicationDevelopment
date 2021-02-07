using System;
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

namespace App_Dev.Areas.Authenticated.Controllers
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
        // GET
        public IActionResult Index()
        {
            return View();
        }
        // GET
        public async Task<IActionResult> Enrollment()
        {
            var traineeList = await _unitOfWork.TraineeProfile.GetAllAsync();
            var enrollsList = await _unitOfWork.Enrollment.GetAllAsync();
            EnrollmentViewModel enrollVm = new EnrollmentViewModel()
            {
                TraineeList = traineeList,
                EnrollList = enrollsList
            };
            return View(enrollVm);
        }
    }
}