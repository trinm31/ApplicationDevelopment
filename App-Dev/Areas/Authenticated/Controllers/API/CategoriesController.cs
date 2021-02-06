using App_Dev.DataAccess.Repository.IRepository;
using App_Dev.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App_Dev.Areas.Authenticated.Controllers.Api
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
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allObj = await _unitOfWork.CourseCategory.GetAllAsync();
            return Json(new { data = allObj });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var objFromDb = await _unitOfWork.CourseCategory.GetAsync(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }

            await _unitOfWork.CourseCategory.RemoveAsync(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }
    }
}
