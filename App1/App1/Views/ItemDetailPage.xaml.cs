using System.ComponentModel;

using App1.Models;
using App1.ViewModels;

using Xamarin.Forms;

namespace App1.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemDetailPage : ContentPage
    {
        private readonly ItemDetailViewModel viewModel;

        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
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
            if (await DisplayAlert("Are you sure you want to delete this item?", string.Empty, "Yes", "No"))
            {
                MessagingCenter.Send(this, "DeleteItem", viewModel.Item);
                await Navigation.PopAsync();
            }
        }
    }
}
