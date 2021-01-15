using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App_Dev.Models
{
    public class Course
    {
        [Key] 
        public int Id { get; set; }
        [Display(Name = "Category Name")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required] 
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public CourseCategory Category { get; set; }
        
    }
}