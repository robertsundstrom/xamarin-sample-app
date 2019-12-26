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

        [Fact(DisplayName = "Cannot login when values are default")]
        public void CannotLoginWhenValuesAreDefault()
        {
            var loginViewModel = new LoginViewModel(
                Fixture.NavigationServiceMock.Object,
                Fixture.IdentityServiceMock.Object,
                Fixture.NativeCallsMock.Object);

            Assert.False(loginViewModel.HasErrors);
            Assert.False(loginViewModel.LoginCommand.CanExecute(null));
        }

        [Fact(DisplayName = "Cannot login when inputs are invalid")]
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
            Assert.False(loginViewModel.LoginCommand.CanExecute(null));
        }

        [Fact(DisplayName = "Cannot login when email is not set")]
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
            Assert.False(loginViewModel.LoginCommand.CanExecute(null));
        }

        [Fact(DisplayName = "Cannot login when password is not set")]
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
            Assert.False(loginViewModel.LoginCommand.CanExecute(null));
        }

        [Fact(DisplayName = "Can login when all inputs are valid")]
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
            Assert.True(loginViewModel.LoginCommand.CanExecute(null));
        }

        [Fact(DisplayName = "Login is successful when all values are valid")]
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

        [Fact(DisplayName = "Is navigating to AppShell on successful login")]
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

        [Fact(DisplayName = "Is showing toast on unsuccessful login")]
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

        [Fact(DisplayName = "Is navigating to registration on command executed")]
        public void IsNavigatingToRegistrationOnCommandExecuted()
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
