using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using App_Dev.DataAccess.Repository.IRepository;
using App_Dev.Models;
using App_Dev.Models.ViewModels;
using App_Dev.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App_Dev.Areas.Authenticated.Controllers
{
    [Area("Authenticated")]
    [Authorize(Roles = SD.Role_Staff)]
    public class CategoriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment Environment;

        public CategoriesController(IUnitOfWork unitOfWork,  IWebHostEnvironment _environment)
        {
            _unitOfWork = unitOfWork;
            Environment = _environment;
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
        
        public async Task<IActionResult> UploadFile()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadFile(List<IFormFile> files)
        {
            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;

            string path = Path.Combine(this.Environment.WebRootPath, "uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            List<string> uploadedFiles = new List<string>();
            foreach (IFormFile postedFile in files)
            {
                string fileName = Path.GetFileName(postedFile.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                    ViewBag.Message += fileName+",";
                }
            }
            return View();
        }
    }
}