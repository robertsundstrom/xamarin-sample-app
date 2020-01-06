using App1.Data;
using App1.Services;

using Microsoft.EntityFrameworkCore;

using Moq;

namespace App1.Tests
{
    public class AppShellViewModelFixture
    {
        public AppShellViewModelFixture()
        {
            NavigationServiceMock = new Mock<INavigationService>();
            IdentityServiceMock = new Mock<IIdentityService>();
            LocalizationServiceMock = new Mock<ILocalizationService>();
            AlertServiceMock = new Mock<IAlertService>();
            ApplicationDbContext = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase("db")
                    .Options);
        }

        public Mock<INavigationService> NavigationServiceMock { get; }
        public Mock<IIdentityService> IdentityServiceMock { get; }
        public Mock<ILocalizationService> LocalizationServiceMock { get; }
        public Mock<IAlertService> AlertServiceMock { get; set; }
        public ApplicationDbContext ApplicationDbContext { get; }

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
