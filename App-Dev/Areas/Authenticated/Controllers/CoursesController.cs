using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App_Dev.DataAccess.Repository.IRepository;
using App_Dev.Models;
using App_Dev.Models.ViewModels;
using App_Dev.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App_Dev.Areas.Authenticated.Controllers
{
    [Area("Authenticated")]
    [Authorize(Roles = SD.Role_Staff)]
    public class CoursesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoursesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // GET
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<CourseCategory> CateLists = await _unitOfWork.CourseCategory.GetAllAsync();
            CourseViewModel courseVm = new CourseViewModel()
            {
                Course = new Course(),
                CategoryList = CateLists.Select(I => new SelectListItem
                {
                    Text = I.Name,
                    Value = I.Id.ToString()
                })
            };
            if (id == null)
            {
                return View(courseVm);
            }

            courseVm.Course = await _unitOfWork.Course.GetAsync(id.GetValueOrDefault());
            if (courseVm.Course == null)
            {
                return NotFound();
            }
            return View(courseVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(CourseViewModel courseVm)
        {
            IEnumerable<CourseCategory> CateLists = await _unitOfWork.CourseCategory.GetAllAsync();
            if (ModelState.IsValid)
            {
                var nameFromDb =
                    await _unitOfWork.Course.GetAllAsync(c => c.Name == courseVm.Course.Name && c.Id != courseVm.Course.Id);
                if (courseVm.Course.Id == 0 && !nameFromDb.Any())
                {
                    await _unitOfWork.Course.AddAsync(courseVm.Course);
                }
                else if (courseVm.Course.Id != 0 && !nameFromDb.Any())
                {
                   await _unitOfWork.Course.Update(courseVm.Course);
                }
                else
                {
                    ViewData["Message"] = "Error: Name already exists";
                    courseVm = new CourseViewModel()
                    {
                        Course = new Course(),
                        CategoryList = CateLists.Select(I => new SelectListItem
                        {
                            Text = I.Name,
                            Value = I.Id.ToString()
                        })
                    };
                    return View(courseVm);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
           
            courseVm = new CourseViewModel()
            {
                Course = new Course(),
                CategoryList = CateLists.Select(I => new SelectListItem
                {
                    Text = I.Name,
                    Value = I.Id.ToString()
                })
            };
            return View(courseVm);
        }
    }
}