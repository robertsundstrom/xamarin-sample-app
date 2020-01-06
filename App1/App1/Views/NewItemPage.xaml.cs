using System;
using System.ComponentModel;

using App1.ViewModels;

using Xamarin.Forms;

namespace App1.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class NewItemPage : ContentPage
    {
        private NewItemViewModel ViewModel => (NewItemViewModel)BindingContext;

        public NewItemPage()
        {
            InitializeComponent();

            BindingContext = new NewItemViewModel();
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            if (ViewModel.CanSubmit)
            {
                var newItem = new Models.Item()
                {
                    Text = ViewModel.Text,
                    Description = ViewModel.Description
                };
                MessagingCenter.Send(this, "AddItem", newItem);
                await Navigation.PopModalAsync();
            }
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
