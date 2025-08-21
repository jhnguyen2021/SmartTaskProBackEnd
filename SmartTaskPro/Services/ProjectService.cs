using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SmartTaskPro.Data;
using SmartTaskPro.DTOs;
using SmartTaskPro.Models;
using SmartTaskPro.Services.Interfaces;

namespace SmartTaskPro.Services;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;
    public ProjectService(AppDbContext db, IMapper mapper)
    {
        _db = db; _mapper = mapper;
    }

    public async Task<ProjectResponseDto> CreateAsync(Guid ownerId, ProjectCreateDto dto)
    {
        var entity = _mapper.Map<Project>(dto);
        entity.OwnerId = ownerId;
        _db.Projects.Add(entity);
        await _db.SaveChangesAsync();
        return _mapper.Map<ProjectResponseDto>(entity);
    }

    public Task<ProjectResponseDto?> GetAsync(Guid id) =>
        _db.Projects.Where(p => p.Id == id)
            .ProjectTo<ProjectResponseDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<ProjectResponseDto>> GetByOwnerAsync(Guid ownerId) =>
        await _db.Projects.Where(p => p.OwnerId == ownerId)
            .ProjectTo<ProjectResponseDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

    public async Task<ProjectResponseDto?> UpdateAsync(Guid id, ProjectUpdateDto dto)
    {
        var entity = await _db.Projects.FindAsync(id);
        if (entity is null) return null;
        _mapper.Map(dto, entity);
        await _db.SaveChangesAsync();
        return _mapper.Map<ProjectResponseDto>(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _db.Projects.FindAsync(id);
        if (entity is null) return false;
        _db.Projects.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}
