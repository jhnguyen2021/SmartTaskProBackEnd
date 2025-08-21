using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartTaskPro.DTOs;
using SmartTaskPro.Services.Interfaces;

namespace SmartTaskPro.Controllers;

[ApiController]
[Route("api/projects/{projectId:guid}/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _svc;
    public TasksController(ITaskService svc) => _svc = svc;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskResponseDto>>> List(Guid projectId, [FromQuery] string? status)
        => Ok(await _svc.GetByProjectAsync(projectId, status));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TaskResponseDto>> Get(Guid projectId, Guid id)
    {
        var t = await _svc.GetAsync(id);
        return t is null || t.ProjectId != projectId ? NotFound() : Ok(t);
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponseDto>> Create(Guid projectId, TaskCreateDto dto)
    {
        var created = await _svc.CreateAsync(projectId, dto);
        return CreatedAtAction(nameof(Get), new { projectId, id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TaskResponseDto>> Update(Guid projectId, Guid id, TaskUpdateDto dto)
    {
        var updated = await _svc.UpdateAsync(id, dto);
        return updated is null || updated.ProjectId != projectId ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid projectId, Guid id)
        => await _svc.DeleteAsync(id) ? NoContent() : NotFound();
}
