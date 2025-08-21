using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SmartTaskPro.Data;
using SmartTaskPro.DTOs;
using SmartTaskPro.Models;
using SmartTaskPro.Services.Interfaces;
using TaskStatusEnum = SmartTaskPro.Models.Enums.WorkStatus;

namespace SmartTaskPro.Services;

public class TaskService : ITaskService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;
    public TaskService(AppDbContext db, IMapper mapper)
    {
        _db = db; _mapper = mapper;
    }

    public async Task<TaskResponseDto> CreateAsync(Guid projectId, TaskCreateDto dto)
    {
        var entity = _mapper.Map<TaskItem>(dto);
        entity.ProjectId = projectId;
        _db.TaskItems.Add(entity);
        await _db.SaveChangesAsync();
        return _mapper.Map<TaskResponseDto>(entity);
    }

    public Task<TaskResponseDto?> GetAsync(Guid id) =>
        _db.TaskItems.Where(t => t.Id == id)
            .ProjectTo<TaskResponseDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<TaskResponseDto>> GetByProjectAsync(Guid projectId, string? status = null)
    {
        var q = _db.TaskItems.Where(t => t.ProjectId == projectId);
        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<TaskStatusEnum>(status, true, out var s))
            q = q.Where(t => t.Status == s);

        return await q.ProjectTo<TaskResponseDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<TaskResponseDto?> UpdateAsync(Guid id, TaskUpdateDto dto)
    {
        var entity = await _db.TaskItems.FindAsync(id);
        if (entity is null) return null;
        _mapper.Map(dto, entity);
        await _db.SaveChangesAsync();
        return _mapper.Map<TaskResponseDto>(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _db.TaskItems.FindAsync(id);
        if (entity is null) return false;
        _db.TaskItems.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}
