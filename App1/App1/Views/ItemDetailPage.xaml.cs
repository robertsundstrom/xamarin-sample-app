using System.ComponentModel;

using App1.Models;
using App1.Services;
using App1.ViewModels;

using Microsoft.Extensions.DependencyInjection;

using Xamarin.Forms;

namespace App1.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemDetailPage : ContentPage
    {
        private readonly ItemDetailViewModel viewModel;
        private readonly ILocalizationService localizationService;

        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;

            localizationService = App.ServiceProvider.GetService<ILocalizationService>();
        }

        public ItemDetailPage()
        {
            InitializeComponent();

            var item = new Item
            {
                Text = "Item 1",
                Description = "This is an item description."
            };

            viewModel = new ItemDetailViewModel(item);
            BindingContext = viewModel;
        }

        private async void Delete_Clicked(object sender, System.EventArgs e)
        {
            if (await DisplayAlert(
                localizationService.GetString("SectionItemDetailsDeleteConfirmationText"),
                string.Empty,
                localizationService.GetString("SectionItemDetailsDeleteConfirmationTextYes"),
                localizationService.GetString("SectionItemDetailsDeleteConfirmationTextNo")))
            {
                MessagingCenter.Send(this, "DeleteItem", viewModel.Item);
                await Navigation.PopAsync();
            }
        }
    }
}
