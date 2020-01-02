using App1.Services;

using Moq;

namespace App1.Tests
{
    public class LoginViewModelFixture
    {
        public LoginViewModelFixture()
        {
            NavigationServiceMock = new Mock<INavigationService>();
            IdentityServiceMock = new Mock<IIdentityService>();
            LocalizationServiceMock = new Mock<ILocalizationService>();
            NativeCallsMock = new Mock<INativeCalls>();
        }

        public Mock<INavigationService> NavigationServiceMock { get; }
        public Mock<IIdentityService> IdentityServiceMock { get; }
        public Mock<ILocalizationService> LocalizationServiceMock { get; }
        public Mock<INativeCalls> NativeCallsMock { get; }

        public void Reset()
        {
            IdentityServiceMock
                .Setup(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            NavigationServiceMock.Invocations.Clear();
            IdentityServiceMock.Invocations.Clear();
            LocalizationServiceMock.Invocations.Clear();
            NativeCallsMock.Invocations.Clear();
        }
    }
}
