using App1.Services;
using App1.ViewModels;

using AutoMapper;

using Moq;

namespace App1.Tests
{
    public class RegistrationViewModelFixture
    {
        public RegistrationViewModelFixture()
        {
            NavigationServiceMock = new Mock<INavigationService>();
            IdentityServiceMock = new Mock<IIdentityService>();
            LocalizationServiceMock = new Mock<ILocalizationService>();
            AlertServiceMock = new Mock<IAlertService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RegistrationViewModel, RegistrationModel>();
            });

            Mapper = config.CreateMapper();

            Initialize();
        }

        public Mock<INavigationService> NavigationServiceMock { get; }
        public Mock<IIdentityService> IdentityServiceMock { get; }
        public Mock<ILocalizationService> LocalizationServiceMock { get; }
        public Mock<IAlertService> AlertServiceMock { get; }
        public IMapper Mapper { get; }


        public void Initialize()
        {
            AlertServiceExtensions.LocalizationService = LocalizationServiceMock.Object;

            IdentityServiceMock
                .Setup(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            LocalizationServiceMock
                .Setup(x => x.GetString(It.IsAny<string>()))
                .Returns(string.Empty);
        }

        public void Reset()
        {
            Initialize();

            NavigationServiceMock.Invocations.Clear();
            IdentityServiceMock.Invocations.Clear();
            LocalizationServiceMock.Invocations.Clear();
            AlertServiceMock.Invocations.Clear();
        }
    }
}
