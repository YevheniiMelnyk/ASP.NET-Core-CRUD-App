using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Student
    {
        [Required]
        [Key]
        public int STUDENT_ID { get; set; }

        [Required]
        public int GROUP_ID { get; set; }

        [Required]
        [Display(Name = "Student first name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The length of the student first name must be between 2 and 50 characters.")]
        public string FIRST_NAME { get; set; }

        [Required]
        [Display(Name = "Student last name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The length of the student last name must be between 2 and 50 characters.")]
        public string LAST_NAME { get; set; }
    }
}