﻿using System;
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
    public class OverviewController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        public OverviewController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var allcourse = await _unitOfWork.Course.GetAllAsync(includeProperties: "CourseCategory");
            return View(allcourse);
        }
        public async Task<IActionResult> Overview(int? id)
        {
            var trainerFromDb = await _unitOfWork.CourseTrainer.GetAllAsync(u => u.CourseId == id, includeProperties:"TrainerProfile");
            var traineeFromDb = await _unitOfWork.Enrollment.GetAllAsync(e => e.CourseId == id && e.EnrollStatus == SD.Approve, includeProperties:"TraineeProfile");
            OverviewViewModel overviewVM = new OverviewViewModel()
            {
                TrainerList = trainerFromDb,
                TraineeList = traineeFromDb
            };
            return View(overviewVM);
        }
       
    }
}
