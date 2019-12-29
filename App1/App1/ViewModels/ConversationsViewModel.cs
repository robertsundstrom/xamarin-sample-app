using System.Collections.ObjectModel;
using System.Threading.Tasks;

using App1.MobileAppService.Client;
using App1.Resources;
using App1.Services;

using Xamarin.Forms;

namespace App1.ViewModels
{
    public class ConversationsViewModel : ViewModelBase
    {
        private readonly IConversationsClient conversationsClient;
        private readonly ILocalizationService localizationService;
        private readonly IAlertService alertService;
        private const int ConversationNameMaxLength = 20;
        private Conversation? selectedConversation = null;

        public ConversationsViewModel(IConversationsClient conversationsClient, INavigationService navigationService, ILocalizationService localizationService, IAlertService alertService)
        {
            this.conversationsClient = conversationsClient;
            this.localizationService = localizationService;
            this.alertService = alertService;

            Conversations = new ObservableCollection<Conversation>();

            StartConversationCommand = new Command(async () =>
            {
                string name = await alertService.DisplayPromptAsync(
                    localizationService.GetString(nameof(AppResources.SectionConversationsModalStartConversationTitle)),
                    localizationService.GetString(nameof(AppResources.SectionConversationsModalStartConversationDescription)),
                    localizationService.GetString(nameof(AppResources.SectionConversationsModalStartConversationButtonStart)),
                    localizationService.GetString(nameof(AppResources.SectionConversationsModalStartConversationButtonCancel)),
                    localizationService.GetString(nameof(AppResources.SectionConversationsModalStartConversationNamePlaceholder)),
                    ConversationNameMaxLength);
                if (name != null)
                {
                    var conversation = await conversationsClient.StartConversationAsync(name);
                    Conversations.Add(conversation);
                    ViewConversationCommand.Execute(conversation);
                }
            });
            RefreshConversationsListsCommand = new Command(async () => await RefreshConversationList());
            ViewConversationCommand = new Command<Conversation>(async conversation =>
            {
                if (conversation == null)
                {
                    return;
                }
                await navigationService.PushAsync<ConversationViewModel>(conversation);
                SelectedConversation = null!;
            });
        }

        public override async Task InitializeAsync(object? arg)
        {
            await RefreshConversationList();
        }

        private async Task RefreshConversationList()
        {
            IsBusy = true;

            try
            {
                var conversations = await conversationsClient.GetConversationsAsync();
                await Device.InvokeOnMainThreadAsync(() =>
                {
                    Conversations.Clear();
                    foreach (var conversation in conversations)
                    {
                        Conversations.Add(conversation);
                    }
                });
            }
            finally
            {
                IsBusy = false;
            }
        }

        public Conversation? SelectedConversation
        {
            get => selectedConversation;
            set => SetProperty(ref selectedConversation, value);
        }

        public ObservableCollection<Conversation> Conversations { get; }

        public Command StartConversationCommand { get; }

        public Command RefreshConversationsListsCommand { get; }

        public Command ViewConversationCommand { get; }
    }
}
