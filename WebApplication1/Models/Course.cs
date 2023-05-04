using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Course
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int COURSE_ID { get; set; }

        [Required]
        [Display(Name = "Course name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The length of the name must be between 3 and 50 characters.")]
        [Remote("IsNameExist", "Home", ErrorMessage = "A course with this name already exists.")]
        public string NAME { get; set; }

        [Required]
        [Display(Name = "Course description")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The length of the description must be between 3 and 50 characters.")]
        public string DESCRIPTION { get; set; }
    }
}
