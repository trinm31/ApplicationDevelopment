using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App_Dev.Models.ViewModels
{
    public class TrainerAssignVM
    {
        [Required]
        public Course Course { get; set; }
        [Required]
        public CourseAssignToTrainer CourseAssignToTrainer { get; set; }
        public IEnumerable<SelectListItem> TrainerList { get; set; }
    }
}