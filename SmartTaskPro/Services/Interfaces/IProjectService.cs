using SmartTaskPro.DTOs;
namespace SmartTaskPro.Services.Interfaces;



public interface IProjectService
{
    Task<ProjectResponseDto> CreateAsync(Guid ownerId, ProjectCreateDto dto);
    Task<ProjectResponseDto?> GetAsync(Guid id);
    Task<IEnumerable<ProjectResponseDto>> GetByOwnerAsync(Guid ownerId);
    Task<ProjectResponseDto?> UpdateAsync(Guid id, ProjectUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}