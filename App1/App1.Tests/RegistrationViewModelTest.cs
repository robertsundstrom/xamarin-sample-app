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

        [Fact(DisplayName = "Cannot register when values are default")]
        public void CannotRegisterWhenValuesAreDefault()
        {
            var registrationViewModel = new RegistrationViewModel(
                Fixture.IdentityServiceMock.Object,
                Fixture.NavigationServiceMock.Object,
                Fixture.NativeCallsMock.Object);

            Assert.False(registrationViewModel.HasErrors);
            Assert.False(registrationViewModel.RegisterCommand.CanExecute(null));
        }


        [Fact(DisplayName = "Is showing toast on exception")]
        public void IsShowingToastOnException()
        {
            Fixture.IdentityServiceMock.Invocations.Clear();
            Fixture.NavigationServiceMock.Invocations.Clear();
            Fixture.NativeCallsMock.Invocations.Clear();

            Fixture.IdentityServiceMock
                .Setup(x => x.RegisterAsync(It.IsAny<RegistrationModel>()))
                .Throws(new App1.MobileAppService.Client.ApiException(string.Empty, 0, string.Empty, null, null));

            var registrationViewModel = new RegistrationViewModel(
                Fixture.IdentityServiceMock.Object,
                Fixture.NavigationServiceMock.Object,
                Fixture.NativeCallsMock.Object)
            {
                Email = "test@test.com",
                Password = "foo",
                FirstName = "Anders",
                LastName = "Andersson"
            };

            registrationViewModel.RegisterCommand.Execute(null);

            Fixture.NativeCallsMock.Verify(x => x.OpenToast(It.IsAny<string>()), Times.Once);
        }

        [Fact(DisplayName = "Is navigating to AppShell on successful registration")]
        public void IsNavigatingToAppShellOnSuccessfulRegistration()
        {
            Fixture.IdentityServiceMock.Invocations.Clear();
            Fixture.NavigationServiceMock.Invocations.Clear();

            Fixture.IdentityServiceMock
                .Setup(x => x.RegisterAsync(It.IsAny<RegistrationModel>()));

            var registrationViewModel = new RegistrationViewModel(
                Fixture.IdentityServiceMock.Object,
                Fixture.NavigationServiceMock.Object,
                Fixture.NativeCallsMock.Object)
            {
                Email = "test@test.com",
                Password = "foo",
                FirstName = "Anders",
                LastName = "Andersson"
            };

            registrationViewModel.RegisterCommand.Execute(null);

            Fixture.NavigationServiceMock.Verify(x => x.PushAsync<AppShellViewModel>(), Times.Once);
        }
    }
}
