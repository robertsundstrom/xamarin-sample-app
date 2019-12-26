using App1.ViewModels;

using Moq;

using Xunit;

namespace App1.Tests
{
    public class LoginViewModelTest : IClassFixture<LoginViewModelFixture>
    {
        public LoginViewModelFixture Fixture { get; }

        public LoginViewModelTest(LoginViewModelFixture fixture)
        {
            Fixture = fixture;
        }

        [Fact]
        public void CannotLoginWhenValuesAreDefault()
        {
            var loginViewModel = new LoginViewModel(
                Fixture.NavigationServiceMock.Object,
                Fixture.IdentityServiceMock.Object,
                Fixture.NativeCallsMock.Object);

            Assert.False(loginViewModel.HasErrors);
            Assert.False(loginViewModel.CanSubmit);
            Assert.False(loginViewModel.LoginCommand.CanExecute(null));
        }

        [Fact]
        public void CannotLoginWhenInputsAreInvalid()
        {
            var loginViewModel = new LoginViewModel(
                Fixture.NavigationServiceMock.Object,
                Fixture.IdentityServiceMock.Object,
                Fixture.NativeCallsMock.Object)
            {
                Email = "",
                Password = ""
            };

            Assert.True(loginViewModel.HasErrors);
            Assert.False(loginViewModel.CanSubmit);
            Assert.False(loginViewModel.LoginCommand.CanExecute(null));
        }

        [Fact]
        public void CannotLoginWhenEmailIsNotSet()
        {
            var loginViewModel = new LoginViewModel(
                Fixture.NavigationServiceMock.Object,
                Fixture.IdentityServiceMock.Object,
                Fixture.NativeCallsMock.Object)
            {
                Email = "",
                Password = "foo"
            };

            Assert.True(loginViewModel.HasErrors);
            Assert.False(loginViewModel.CanSubmit);
            Assert.False(loginViewModel.LoginCommand.CanExecute(null));
        }

        [Fact]
        public void CannotLoginWhenPasswordIsNotSet()
        {
            var loginViewModel = new LoginViewModel(
                Fixture.NavigationServiceMock.Object,
                Fixture.IdentityServiceMock.Object,
                Fixture.NativeCallsMock.Object)
            {
                Email = "test@test.com",
                Password = ""
            };

            Assert.True(loginViewModel.HasErrors);
            Assert.False(loginViewModel.CanSubmit);
            Assert.False(loginViewModel.LoginCommand.CanExecute(null));
        }

        [Fact]
        public void CanLoginWhenAllInputsAreValid()
        {
            var loginViewModel = new LoginViewModel(
                Fixture.NavigationServiceMock.Object,
                Fixture.IdentityServiceMock.Object,
                Fixture.NativeCallsMock.Object)
            {
                Email = "test@test.com",
                Password = "foo"
            };

            Assert.False(loginViewModel.HasErrors);
            Assert.True(loginViewModel.CanSubmit);
            Assert.True(loginViewModel.LoginCommand.CanExecute(null));
        }

        [Fact]
        public void LoginIsSuccessfulWhenAllValuesAreValid()
        {
            Fixture.IdentityServiceMock.Invocations.Clear();
            Fixture.NavigationServiceMock.Invocations.Clear();

            Fixture.IdentityServiceMock
                .Setup(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var loginViewModel = new LoginViewModel(
                Fixture.NavigationServiceMock.Object,
                Fixture.IdentityServiceMock.Object,
                Fixture.NativeCallsMock.Object)
            {
                Email = "test@test.com",
                Password = "foo"
            };

            loginViewModel.LoginCommand.Execute(null);


            Fixture.IdentityServiceMock.Verify(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void IsNavigatingToAppShellOnSuccessfulLogin()
        {
            Fixture.IdentityServiceMock.Invocations.Clear();
            Fixture.NavigationServiceMock.Invocations.Clear();

            Fixture.IdentityServiceMock
                .Setup(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var loginViewModel = new LoginViewModel(
                Fixture.NavigationServiceMock.Object,
                Fixture.IdentityServiceMock.Object,
                Fixture.NativeCallsMock.Object)
            {
                Email = "test@test.com",
                Password = "foo"
            };

            loginViewModel.LoginCommand.Execute(null);

            Fixture.NavigationServiceMock.Verify(x => x.PushAsync<AppShellViewModel>(), Times.Once);
        }

        [Fact]
        public void IsShowingToastOnUnsuccessfulLogin()
        {
            Fixture.IdentityServiceMock.Invocations.Clear();
            Fixture.NavigationServiceMock.Invocations.Clear();

            Fixture.IdentityServiceMock
                .Setup(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var loginViewModel = new LoginViewModel(
                 Fixture.NavigationServiceMock.Object,
                 Fixture.IdentityServiceMock.Object,
                 Fixture.NativeCallsMock.Object)
            {
                Email = "test@test.com",
                Password = "foo"
            };

            loginViewModel.LoginCommand.Execute(null);

            Fixture.NativeCallsMock.Verify(x => x.OpenToast(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void IsNavigatingToRegistration()
        {
            Fixture.IdentityServiceMock.Invocations.Clear();
            Fixture.NavigationServiceMock.Invocations.Clear();

            Fixture.IdentityServiceMock
                .Setup(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var loginViewModel = new LoginViewModel(
                Fixture.NavigationServiceMock.Object,
                Fixture.IdentityServiceMock.Object,
                Fixture.NativeCallsMock.Object)
            {
                Email = "",
                Password = ""
            };

            loginViewModel.NavigateToRegistrationPageCommand.Execute(null);

            Fixture.NavigationServiceMock.Verify(x => x.PushAsync<RegistrationViewModel>(), Times.Once);
        }
    }
}
