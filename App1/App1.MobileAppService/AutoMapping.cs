using App1.MobileAppService.Users;

using AutoMapper;

namespace App1.MobileAppService
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Models.User, Users.User>();
            CreateMap<UpdateUserProfileRequest, Models.User>();
        }
    }
}
