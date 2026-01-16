using AutoMapper;
using Shared.DTO.Users;
using UserEntity = Domain.Entities.Users.User;

namespace Application.Common.Mappings.User;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<UserEntity, UserResponseRequest>();
    }
}
