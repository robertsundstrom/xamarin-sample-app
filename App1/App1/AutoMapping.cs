using App1.MobileAppService.Client;
using App1.ViewModels;

using AutoMapper;

namespace App1
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Models.Item, Item>();
            CreateMap<Item, Models.Item>();

            CreateMap<RegistrationClient, RegistrationClient>();
            CreateMap<EditUserProfileViewModel, UpdateUser>();
        }
    }
}
