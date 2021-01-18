using System.ComponentModel.DataAnnotations;

namespace App_Dev.Models
{
    public class CourseCategory
    {
        [Key] 
        public int Id { get; set; }
        [Display(Name = "Category Name")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}