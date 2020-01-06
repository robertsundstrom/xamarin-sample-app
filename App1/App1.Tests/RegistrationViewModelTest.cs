using App1.Services;
using App1.ViewModels;

using Moq;

using Xunit;

namespace App1.Tests
{
    public class RegistrationViewModelTest : IClassFixture<RegistrationViewModelFixture>
    {
        public RegistrationViewModelFixture Fixture { get; }

        public RegistrationViewModelTest(RegistrationViewModelFixture fixture)
        {
            Fixture = fixture;
        }

        [Fact(DisplayName = "Cannot register when form is pristine")]
        public void CannotRegisterWhenFormIsPristine()
        {
            Fixture.Reset();

            var registrationViewModel = new RegistrationViewModel(
                Fixture.IdentityServiceMock.Object,
                Fixture.NavigationServiceMock.Object,
                Fixture.LocalizationServiceMock.Object,
                Fixture.AlertServiceMock.Object,
                Fixture.Mapper);

            Assert.True(registrationViewModel.IsPristine);
            Assert.False(registrationViewModel.HasErrors);
            Assert.False(registrationViewModel.RegisterCommand.CanExecute(null));
        }


        [Fact(DisplayName = "Is showing toast on ApiException")]
        public void IsShowingToastOnApiException()
        {
            Fixture.Reset();

            Fixture.IdentityServiceMock
                .Setup(x => x.RegisterAsync(It.IsAny<RegistrationModel>()))
                .Throws(new App1.MobileAppService.Client.ApiException(string.Empty, 0, string.Empty, null, null));

            var registrationViewModel = new RegistrationViewModel(
                Fixture.IdentityServiceMock.Object,
                Fixture.NavigationServiceMock.Object,
                Fixture.LocalizationServiceMock.Object,
                Fixture.AlertServiceMock.Object,
                Fixture.Mapper)
            {
                Email = "test@test.com",
                Password = "Abc123!?",
                ConfirmPassword = "Abc123!?",
                FirstName = "Anders",
                LastName = "Andersson",
                IsAcceptingUserAgreement = true
            };

            registrationViewModel.RegisterCommand.Execute(null);

            Fixture.AlertServiceMock.Verify(x => x.DisplayAlertAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact(DisplayName = "Password and ConfirmPassword not equal")]
        public void PasswordAndConfirmPasswordNotEqual()
        {
            Fixture.Reset();

            var registrationViewModel = new RegistrationViewModel(
                Fixture.IdentityServiceMock.Object,
                Fixture.NavigationServiceMock.Object,
                Fixture.LocalizationServiceMock.Object,
                Fixture.AlertServiceMock.Object,
                Fixture.Mapper)
            {
                Email = "test@test.com",
                Password = "Abc123!?",
                ConfirmPassword = "Hello",
                FirstName = "Anders",
                LastName = "Andersson",
                IsAcceptingUserAgreement = true
            };

            registrationViewModel.RegisterCommand.Execute(null);
        }

        [Fact(DisplayName = "Is navigating to AppShell on successful registration")]
        public void IsNavigatingToAppShellOnSuccessfulRegistration()
        {
            Fixture.Reset();

            Fixture.IdentityServiceMock
                .Setup(x => x.RegisterAsync(It.IsAny<RegistrationModel>()));

            var registrationViewModel = new RegistrationViewModel(
                Fixture.IdentityServiceMock.Object,
                Fixture.NavigationServiceMock.Object,
                Fixture.LocalizationServiceMock.Object,
                Fixture.AlertServiceMock.Object,
                Fixture.Mapper)
            {
                Email = "test@test.com",
                Password = "Abc123!?",
                ConfirmPassword = "Abc123!?",
                FirstName = "Anders",
                LastName = "Andersson",
                IsAcceptingUserAgreement = true
            };

            registrationViewModel.RegisterCommand.Execute(null);

            Assert.False(registrationViewModel.IsPristine);

            Fixture.NavigationServiceMock.Verify(x => x.PushAsync<AppShellViewModel>(null), Times.Once);
        }
    }
}
