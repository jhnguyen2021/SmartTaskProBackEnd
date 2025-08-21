using SmartTaskPro.DTOs;

namespace SmartTaskPro.Services.Interfaces;

public interface ITaskService
{
    Task<TaskResponseDto> CreateAsync(Guid projectId, TaskCreateDto dto);
    Task<TaskResponseDto?> GetAsync(Guid id);
    Task<IEnumerable<TaskResponseDto>> GetByProjectAsync(Guid projectId, string? status = null);
    Task<TaskResponseDto?> UpdateAsync(Guid id, TaskUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}
