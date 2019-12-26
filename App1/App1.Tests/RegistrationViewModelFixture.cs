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
            IdentityServiceMock
                .Setup(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            NativeCallsMock = new Mock<INativeCalls>();
        }

        public Mock<INavigationService> NavigationServiceMock { get; }
        public Mock<IIdentityService> IdentityServiceMock { get; }
        public Mock<INativeCalls> NativeCallsMock { get; private set; }
    }
}
