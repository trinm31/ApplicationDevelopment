using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App_Dev.Models.ViewModels
{
    public class CourseViewModel
    {
        public Course Course { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}