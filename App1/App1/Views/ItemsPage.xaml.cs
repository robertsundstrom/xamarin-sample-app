using System;
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
    public partial class ItemsPage : ContentPage
    {
        private readonly ILocalizationService localizationService;

        private ItemsViewModel ViewModel => (ItemsViewModel)BindingContext;

        public ItemsPage()
        {
            InitializeComponent();

            localizationService = App.ServiceProvider.GetRequiredService<ILocalizationService>();
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
            {
                return;
            }

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        private async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (ViewModel.Items.Count == 0)
            {
                ViewModel.LoadItemsCommand.Execute(null);
            }
        }

        private async void DeleteItem_Clicked(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);

            if (await DisplayAlert(
                localizationService.GetString("SectionItemDetailsDeleteConfirmationText"),
                string.Empty,
                localizationService.GetString("SectionItemDetailsDeleteConfirmationTextYes"),
                localizationService.GetString("SectionItemDetailsDeleteConfirmationTextNo")))
            {
                MessagingCenter.Send(this, "DeleteItem", (Item)mi.CommandParameter);
            }
        }
    }
}
