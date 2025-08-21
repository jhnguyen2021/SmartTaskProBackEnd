
using System.ComponentModel.DataAnnotations;

namespace SmartTaskPro.DTOs;

public record ProjectCreateDto([Required] string Name, string? Description);
public record ProjectUpdateDto([Required] string Name, string? Description);
public record ProjectResponseDto(Guid Id, string Name, string? Description, Guid OwnerId);
