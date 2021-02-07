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
        public IActionResult Index()
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
                if (category.Id == 0 && !nameFromDb.Any())
                {
                    await _unitOfWork.CourseCategory.AddAsync(category);
                }
                else if (category.Id != 0 && !nameFromDb.Any())
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
    }
}