using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Group
    {
        [Key]
        public int GROUP_ID { get; set; }

        [Required]
        public int COURSE_ID { get; set; }

        [Required]
        [Display(Name = "Group name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The length of group must be between 3 and 50 characters.")]
        [Remote("IsNameExist", "Group", ErrorMessage = "A group with this name already exists.")]
        public string NAME { get; set; }
    }
}
