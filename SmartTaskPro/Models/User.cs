using SmartTaskPro.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace SmartTaskPro.Models
{
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public ICollection<Project> Projects { get; set; }
        public ICollection<TaskItem> AssignedTasks { get; set; }
    }
}
