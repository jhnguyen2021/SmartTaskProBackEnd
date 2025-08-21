using SmartTaskPro.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace SmartTaskPro.Models
{
    public class Project : BaseEntity
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }

        // Owner (User who created/owns the project)
        [Required]
        public Guid OwnerId { get; set; }
        public User Owner { get; set; }

        // Navigation: Tasks in this project
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
