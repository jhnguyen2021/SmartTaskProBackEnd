using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartTaskPro.DTOs;
using SmartTaskPro.Services.Interfaces;

namespace SmartTaskPro.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _svc;

    public ProjectsController(IProjectService svc) => _svc = svc;

    [HttpGet("mine")]
    public async Task<ActionResult<IEnumerable<ProjectResponseDto>>> Mine()
    {
        var userId = User.FindFirst("sub")?.Value;
        if (userId is null) return Unauthorized();
        var data = await _svc.GetByOwnerAsync(Guid.Parse(userId));
        return Ok(data);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProjectResponseDto>> Get(Guid id)
    {
        var item = await _svc.GetAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectResponseDto>> Create(ProjectCreateDto dto)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (userId is null) return Unauthorized();
        var created = await _svc.CreateAsync(Guid.Parse(userId), dto);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProjectResponseDto>> Update(Guid id, ProjectUpdateDto dto)
    {
        var updated = await _svc.UpdateAsync(id, dto);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var ok = await _svc.DeleteAsync(id);
        return ok ? NoContent() : NotFound();
    }
}
