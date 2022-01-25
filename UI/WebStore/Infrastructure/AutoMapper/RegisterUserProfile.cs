using AutoMapper;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewModels.Identity;

namespace WebStore.Infrastructure.AutoMapper;

public class RegisterUserProfile : Profile
{
    public RegisterUserProfile()
    {
        CreateMap<RegisterUserViewModel, User>()
           .ForMember(user => user.UserName, opt => opt.MapFrom(model => model.UserName));
    }
}