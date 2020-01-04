using App1.Services;

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
        }

        public Mock<INavigationService> NavigationServiceMock { get; }
        public Mock<IIdentityService> IdentityServiceMock { get; }
        public Mock<ILocalizationService> LocalizationServiceMock { get; }
        public Mock<IAlertService> AlertServiceMock { get; set; }

        public void Reset()
        {
            IdentityServiceMock
                .Setup(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            NavigationServiceMock.Invocations.Clear();
            IdentityServiceMock.Invocations.Clear();
            LocalizationServiceMock.Invocations.Clear();
            AlertServiceMock.Invocations.Clear();
        }
    }
}
