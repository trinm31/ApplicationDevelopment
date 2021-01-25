using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IActionResult> Assign(int? id)
        {
            TrainerAssignVM trainerAssignVm = new TrainerAssignVM();
            if (id == null)
            {
                return View(trainerAssignVm);
            }
            var course = await _unitOfWork.Course.GetAsync(id.GetValueOrDefault());
            if (course == null)
            {
                return NotFound();
            }
            IEnumerable<TrainerProfile> trainerList = await _unitOfWork.TrainerProfile.GetAllAsync();
            trainerAssignVm = new TrainerAssignVM()
            {
                Course = course,
                TrainerList = trainerList.Select(I => new SelectListItem
                {
                    Text = I.Name,
                    Value = I.Id.ToString()
                })
            };
            return View(trainerAssignVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(TrainerAssignVM trainerAssignVm)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            trainerAssignVm.CourseAssignToTrainer.CourseId = trainerAssignVm.Course.Id;
            trainerAssignVm.CourseAssignToTrainer.Time = DateTime.Now;
            if (trainerAssignVm.CourseAssignToTrainer.Id == 0)
            {
                await _unitOfWork.CourseAssignToTrainer.AddAsync(trainerAssignVm.CourseAssignToTrainer);
            }
            else
            {
                await _unitOfWork.CourseAssignToTrainer.Update(trainerAssignVm.CourseAssignToTrainer);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        #region Api Calls

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allObj = await _unitOfWork.Course.GetAllAsync(includeProperties:"CourseCategory");
            return Json(new {data = allObj});
        }
        #endregion
    }
}