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
    public class CourseTrainerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        public CourseTrainerController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        // GET
        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> CourseTrainer()
        {
            var trainerList = await _unitOfWork.TrainerProfile.GetAllAsync();
            var assignList = await _unitOfWork.CourseTrainer.GetAllAsync();
            CourseTrainerViewModel courseTrainerVM = new CourseTrainerViewModel()
            {
                TrainerList = trainerList,
                AssignList = assignList
            };
            return View(courseTrainerVM);
        }
    }
}