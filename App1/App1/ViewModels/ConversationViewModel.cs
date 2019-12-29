using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

using App1.MobileAppService.Client;
using App1.Services;

using Xamarin.Forms;

namespace App1.ViewModels
{
    public class ConversationViewModel : ViewModelBase<Conversation>
    {
        private readonly IMessagesClient messagesClient;
        private readonly IAlertService alertService;
        private readonly IChatNotifier chatNotifier;
        private string? text;
        private IDisposable? disposable;

        public ConversationViewModel(IMessagesClient messagesClient, IAlertService alertService, IChatNotifier chatNotifier)
        {
            this.messagesClient = messagesClient;
            this.alertService = alertService;
            this.chatNotifier = chatNotifier;
            Messages = new ObservableCollection<Message>();

            PostMessageCommand = new Command(async () =>
            {
                try
                {
                    //var message = await messagesClient.PostMessageAsync(
                    //    Conversation!.Id,
                    //    new NewMessage
                    //    {
                    //        Text = Text
                    //    });
                    await chatNotifier.SendMessageAsync(new NewMessage
                    {
                        ConversationId = Conversation!.Id,
                        Text = Text
                    });
                    Text = null;
                }
                catch (Exception exc)
                {
                    await alertService.DisplayAlertAsync(string.Empty, exc.Message, "OK");
                }
            }, () => !IsBusy && !string.IsNullOrEmpty(Text));
        }

        public override async Task InitializeAsync(Conversation conversation)
        {
            IsBusy = true;

            try
            {
                Conversation = conversation;
                Title = conversation.Title;

                var messages = await messagesClient.GetMessagesAsync(conversation.Id);

                Messages.Clear();
                foreach (var message in messages)
                {
                    Messages.Add(message);
                }

                disposable = chatNotifier.WhenMessageReceived.Subscribe(m =>
                {
                    if (Guid.Parse(m.Conversation.Id) == conversation.Id)
                    {
                        if (Messages.Any(x => x.Id == m.Id))
                        {
                            return;
                        }
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Messages.Add(m);
                        });
                    }
                });
            }
            finally
            {
                IsBusy = false;
            }
        }

        public string? Text
        {
            get => text;
            set
            {
                SetProperty(ref text, value);
                PostMessageCommand.ChangeCanExecute();
            }
        }

        public ObservableCollection<Message> Messages { get; }
        public Command PostMessageCommand { get; }
        public Conversation? Conversation { get; private set; }
    }
}
