using System.Linq;
using System.Threading.Tasks;
using App_Dev.DataAccess.Repository.IRepository;
using App_Dev.Models;
using App_Dev.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App_Dev.Areas.Authenticated.Controllers
{
    [Area("Authenticated")]
    [Authorize(Roles = SD.Role_Staff)]
    public class CategoriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
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
            CourseCategory courseCategory = new CourseCategory();
            if (id == null)
            {
                return View(courseCategory);
            }

            courseCategory = await _unitOfWork.CourseCategory.GetAsync(id.GetValueOrDefault());
            if (courseCategory == null)
            {
                return NotFound();
            }
            return View(courseCategory);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(CourseCategory category)
        {
            if (ModelState.IsValid)
            {
                var nameFromDb =
                    await _unitOfWork.CourseCategory
                        .GetAllAsync(c => c.Name == category.Name && c.Id != category.Id);
                var isNameExist = nameFromDb.Count() > 0 ? true : false;
                if (category.Id == 0 && !isNameExist )
                {
                    await _unitOfWork.CourseCategory.AddAsync(category);
                }
                else if (category.Id != 0 && !isNameExist)
                {
                    await _unitOfWork.CourseCategory.Update(category);
                }
                else
                {
                    ViewData["Message"] = "Error: Name already exists";
                    return View(category);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        #region Api Calls

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allObj = await _unitOfWork.CourseCategory.GetAllAsync();
            return Json(new {data = allObj});
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var objFromDb = await _unitOfWork.CourseCategory.GetAsync(id);
            if (objFromDb == null)
            {
                return Json(new {success = false, message = "Error while Deleting"});
            }

            await _unitOfWork.CourseCategory.RemoveAsync(objFromDb);
            _unitOfWork.Save();
            return Json(new {success = true, message = "Delete successful"});
        }
        #endregion
    }
}