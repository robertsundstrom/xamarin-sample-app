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

        [Fact(DisplayName = "Does not authenticate when form is pristine")]
        public void DoesNotAunthenticateWhenFormIsPristine()
        {
            Fixture.Reset();

            var loginViewModel = new LoginViewModel(
                Fixture.NavigationServiceMock.Object,
                Fixture.IdentityServiceMock.Object,
                Fixture.LocalizationServiceMock.Object,
                Fixture.AlertServiceMock.Object);

            Assert.False(loginViewModel.IsPristine);
            Assert.False(loginViewModel.HasErrors);

            loginViewModel.LoginCommand.Execute(null);

            Assert.True(loginViewModel.HasErrors);

            Fixture.IdentityServiceMock.Verify(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            Fixture.NavigationServiceMock.Verify(x => x.PushAsync<RegistrationViewModel>(null), Times.Never);
        }

        [Fact(DisplayName = "Cannot login when inputs are invalid")]
        public void CannotLoginWhenInputsAreInvalid()
        {
            Fixture.Reset();

            var loginViewModel = new LoginViewModel(
                Fixture.NavigationServiceMock.Object,
                Fixture.IdentityServiceMock.Object,
                Fixture.LocalizationServiceMock.Object,
                Fixture.AlertServiceMock.Object)
            {
                Email = "ddd@",
                Password = ""
            };

            loginViewModel.LoginCommand.Execute(null);

            Assert.True(loginViewModel.HasErrors);

            Fixture.IdentityServiceMock.Verify(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            Fixture.NavigationServiceMock.Verify(x => x.PushAsync<RegistrationViewModel>(null), Times.Never);
        }

        [Fact(DisplayName = "Does not authenticate when email is empty")]
        public void DoesNotAunthenticateWhenEmailIsEmpty()
        {
            Fixture.Reset();

            var loginViewModel = new LoginViewModel(
                Fixture.NavigationServiceMock.Object,
                Fixture.IdentityServiceMock.Object,
                Fixture.LocalizationServiceMock.Object,
                Fixture.AlertServiceMock.Object)
            {
                Email = "",
                Password = "foo"
            };

            loginViewModel.LoginCommand.Execute(null);

            Assert.True(loginViewModel.HasErrors);

            Fixture.IdentityServiceMock.Verify(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            Fixture.NavigationServiceMock.Verify(x => x.PushAsync<RegistrationViewModel>(null), Times.Never);
        }

        [Fact(DisplayName = "Cannot login when email is invalid")]
        public void CannotLoginWhenEmailIsInvalid()
        {
            Fixture.Reset();

            var loginViewModel = new LoginViewModel(
                Fixture.NavigationServiceMock.Object,
                Fixture.IdentityServiceMock.Object,
                Fixture.LocalizationServiceMock.Object,
                Fixture.AlertServiceMock.Object)
            {
                Email = "foo@",
                Password = "foo"
            };

            loginViewModel.LoginCommand.Execute(null);

            Assert.True(loginViewModel.HasErrors);

            Fixture.IdentityServiceMock.Verify(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            Fixture.NavigationServiceMock.Verify(x => x.PushAsync<RegistrationViewModel>(null), Times.Never);
        }


        [Fact(DisplayName = "Cannot login when password is empty")]
        public void CannotLoginWhenPasswordIsEmpty()
        {
            Fixture.Reset();

            var loginViewModel = new LoginViewModel(
                Fixture.NavigationServiceMock.Object,
                Fixture.IdentityServiceMock.Object,
                Fixture.LocalizationServiceMock.Object,
                Fixture.AlertServiceMock.Object)
            {
                Email = "test@test.com",
                Password = ""
            };

            loginViewModel.LoginCommand.Execute(null);

            Assert.True(loginViewModel.HasErrors);

            Fixture.IdentityServiceMock.Verify(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            Fixture.NavigationServiceMock.Verify(x => x.PushAsync<RegistrationViewModel>(null), Times.Never);
        }

        [Fact(DisplayName = "Is authenticating when all values are valid")]
        public void IsAuthenticatingWhenAllValuesAreValid()
        {
            Fixture.Reset();

            Fixture.IdentityServiceMock
                .Setup(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var loginViewModel = new LoginViewModel(
                Fixture.NavigationServiceMock.Object,
                Fixture.IdentityServiceMock.Object,
                Fixture.LocalizationServiceMock.Object,
                Fixture.AlertServiceMock.Object)
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
            Fixture.Reset();

            Fixture.IdentityServiceMock
                .Setup(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var loginViewModel = new LoginViewModel(
                Fixture.NavigationServiceMock.Object,
                Fixture.IdentityServiceMock.Object,
                Fixture.LocalizationServiceMock.Object,
                Fixture.AlertServiceMock.Object)
            {
                Email = "test@test.com",
                Password = "foo"
            };

            loginViewModel.LoginCommand.Execute(null);

            Fixture.NavigationServiceMock.Verify(x => x.PushAsync<AppShellViewModel>(null), Times.Once);
        }

        [Fact(DisplayName = "Is showing toast on unsuccessful login")]
        public void IsShowingToastOnUnsuccessfulLogin()
        {
            Fixture.Reset();

            Fixture.IdentityServiceMock
                .Setup(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var loginViewModel = new LoginViewModel(
                 Fixture.NavigationServiceMock.Object,
                 Fixture.IdentityServiceMock.Object,
                 Fixture.LocalizationServiceMock.Object,
                 Fixture.AlertServiceMock.Object)
            {
                Email = "test@test.com",
                Password = "foo"
            };

            loginViewModel.LoginCommand.Execute(null);

            Fixture.AlertServiceMock.Verify(x => x.DisplayAlertAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact(DisplayName = "Is navigating to registration on command executed")]
        public void IsNavigatingToRegistrationOnCommandExecuted()
        {
            Fixture.Reset();

            Fixture.IdentityServiceMock
                .Setup(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var loginViewModel = new LoginViewModel(
                Fixture.NavigationServiceMock.Object,
                Fixture.IdentityServiceMock.Object,
                Fixture.LocalizationServiceMock.Object,
                Fixture.AlertServiceMock.Object)
            {
                Email = "",
                Password = ""
            };

            loginViewModel.NavigateToRegistrationPageCommand.Execute(null);

            Fixture.NavigationServiceMock.Verify(x => x.PushAsync<RegistrationViewModel>(null), Times.Once);
        }
    }
}
