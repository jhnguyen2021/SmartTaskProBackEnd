using SmartTaskPro.Models.Base;
using SmartTaskPro.Models.Enums;
using System.ComponentModel.DataAnnotations;



namespace SmartTaskPro.Models
{
    public class TaskItem : BaseEntity
    {
        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [MaxLength(2000)]
        public string? Description { get; set; }

        // ✅ Added back Status & Priority with defaults
        public WorkStatus Status { get; set; } = WorkStatus.Todo;
        public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;

        public DateTime? DueDate { get; set; }
        public DateTime? CompletedAt { get; set; }

        // Relationships
        [Required]
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public Guid? AssignedToId { get; set; }
        public User? AssignedTo { get; set; }
    }
}
