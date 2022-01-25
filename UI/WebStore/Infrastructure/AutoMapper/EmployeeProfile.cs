using AutoMapper;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;

namespace WebStore.Infrastructure.AutoMapper;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<Employee, EmployeeViewModel>()
           .ForMember(ViewModel => ViewModel.Name, opt => opt.MapFrom(Model => Model.FirstName))
           .ReverseMap();
    }
}