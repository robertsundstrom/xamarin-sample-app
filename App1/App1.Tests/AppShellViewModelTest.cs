
using App1.ViewModels;

using Moq;

using Xunit;

namespace App1.Tests
{
    public class AppShellViewModelTest : IClassFixture<AppShellViewModelFixture>
    {
        public AppShellViewModelFixture Fixture { get; }

        public AppShellViewModelTest(AppShellViewModelFixture fixture)
        {
            Fixture = fixture;
        }

        [Fact(DisplayName = "Is logging out on command executed")]
        public void IsLoggingOutOnCommand()
        {
            Fixture.Reset();

            var appShellViewModel = new AppShellViewModel(
                Fixture.IdentityServiceMock.Object,
                Fixture.NavigationServiceMock.Object);

            appShellViewModel.LogOutCommand.Execute(null);

            Fixture.IdentityServiceMock.Verify(x => x.LogOutAsync(), Times.Once);
        }

        [Fact(DisplayName = "Is navigating on log out")]
        public void IsNavigatingOnLogOut()
        {
            Fixture.Reset();

            var appShellViewModel = new AppShellViewModel(
                Fixture.IdentityServiceMock.Object,
                Fixture.NavigationServiceMock.Object);

            appShellViewModel.LogOutCommand.Execute(null);

            Fixture.NavigationServiceMock.Verify(x => x.PushAsync<LoginViewModel>(It.IsAny<LoginViewModelArgs>()), Times.Once);
        }
    }
}
