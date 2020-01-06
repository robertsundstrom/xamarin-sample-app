
using App1.MobileAppService.Models;

using AutoMapper;

namespace App1.MobileAppService
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<UpdateUserDto, User>();
        }
    }
}
