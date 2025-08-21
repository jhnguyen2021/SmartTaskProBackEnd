
using System.ComponentModel.DataAnnotations;
using TaskPriorityEnum = SmartTaskPro.Models.Enums.PriorityLevel;
using TaskStatusEnum = SmartTaskPro.Models.Enums.WorkStatus;

namespace SmartTaskPro.DTOs;


public record TaskCreateDto([Required] string Title, string? Description, TaskPriorityEnum Priority, DateTime? DueDateUtc);
public record TaskUpdateDto([Required] string Title, string? Description, TaskStatusEnum Status, TaskPriorityEnum Priority, DateTime? DueDateUtc, Guid? AssigneeId);
public record TaskResponseDto(Guid Id, Guid ProjectId, string Title, string? Description, TaskStatusEnum Status, TaskPriorityEnum Priority, Guid? AssigneeId, DateTime? DueDateUtc);
