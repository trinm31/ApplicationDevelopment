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
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<CourseCategory> CateLists = await _unitOfWork.CourseCategory.GetAllAsync();
            CourseVM courseVm = new CourseVM()
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
        public async Task<IActionResult> Upsert(CourseVM courseVm)
        {
            if (ModelState.IsValid)
            {
                if (courseVm.Course.Id == 0)
                {
                    await _unitOfWork.Course.AddAsync(courseVm.Course);
                }
                else
                {
                   await _unitOfWork.Course.Update(courseVm.Course);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            IEnumerable<CourseCategory> CateLists = await _unitOfWork.CourseCategory.GetAllAsync();
            courseVm = new CourseVM()
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
        #region Api Calls

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allObj = await _unitOfWork.Course.GetAllAsync(includeProperties:"CourseCategory");
            return Json(new {data = allObj});
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var objFromDb = await _unitOfWork.Course.GetAsync(id);
            if (objFromDb == null)
            {
                return Json(new {success = false, message = "Error while Deleting"});
            }

            await _unitOfWork.Course.RemoveAsync(objFromDb);
            _unitOfWork.Save();
            return Json(new {success = true, message = "Delete successful"});
        }
        #endregion
    }
}