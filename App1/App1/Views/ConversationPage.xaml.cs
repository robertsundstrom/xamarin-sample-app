using App1.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConversationPage : ContentPage
    {
        public ConversationViewModel ConversationViewModel => (ConversationViewModel)BindingContext;

        public ConversationPage()
        {
            InitializeComponent();
        }
    }
}
