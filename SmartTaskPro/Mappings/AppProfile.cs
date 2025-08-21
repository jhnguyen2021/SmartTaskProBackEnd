using AutoMapper;
using SmartTaskPro.DTOs;
using SmartTaskPro.Models;


namespace SmartTaskPro.Mappings
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<Project, ProjectResponseDto>();
            CreateMap<ProjectCreateDto, Project>();
            CreateMap<ProjectUpdateDto, Project>();

            CreateMap<TaskItem, TaskResponseDto>();
            CreateMap<TaskCreateDto, TaskItem>();
            CreateMap<TaskUpdateDto, TaskItem>();
        }
    }
}
