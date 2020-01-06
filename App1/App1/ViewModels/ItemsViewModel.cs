using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using App1.MobileAppService.Client;
using App1.Services;
using App1.Views;

using AutoMapper;

using Xamarin.Forms;

namespace App1.ViewModels
{
    public class ItemsViewModel : ViewModelBase
    {
        private readonly IDataStore<Models.Item> dataStore;
        private readonly IAlertService alertService;
        private readonly IMapper mapper;
        private readonly INavigationService navigationService;
        private readonly IItemsHubClient itemsHubClient;

        public ObservableCollection<Models.Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel(IDataStore<Models.Item> dataStore, IAlertService alertService, IMapper mapper, INavigationService navigationService, IItemsHubClient itemsHubClient)
        {
            Title = "Browse";
            Items = new ObservableCollection<Models.Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            this.dataStore = dataStore;
            this.alertService = alertService;
            this.mapper = mapper;
            this.navigationService = navigationService;
            this.itemsHubClient = itemsHubClient;
            MessagingCenter.Subscribe<NewItemPage, Models.Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Models.Item;
                try
                {
                    if (await dataStore.AddItemAsync(newItem))
                    {
                        if (Items.Any(x => x.Text == item.Text))
                        {
                            Items.Add(newItem);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    await alertService.DisplayAlertOkAsync(string.Empty, ex.Message);
                }
            });

            MessagingCenter.Subscribe<ItemsPage, Models.Item>(this, "DeleteItem", async (obj, item) =>
            {
                await DeleteItem(item);
            });

            MessagingCenter.Subscribe<ItemDetailPage, Models.Item>(this, "DeleteItem", async (obj, item) =>
            {
                await DeleteItem(item);
            });

            itemsHubClient.WhenItemAdded.Subscribe((item) =>
            {
                if (Items.Any(x => x.Text == item.Text))
                {
                    return;
                }
                Items.Add(mapper.Map<Models.Item>(item));
            });

            itemsHubClient.WhenItemDeleted.Subscribe((item) =>
            {
                Items.Remove(
                    Items.FirstOrDefault(x => x.Id == item.Id));
            });

            itemsHubClient.WhenItemUpdated.Subscribe((item) =>
            {
                var index = Items.IndexOf(
                    Items.FirstOrDefault(x => x.Id == item.Id));

                if (index > -1)
                {
                    Items[index] = mapper.Map<Models.Item>(item);
                }
            });
        }

        private async Task DeleteItem(Models.Item item)
        {
            if (item is Models.Item newItem)
            {
                try
                {
                    if (await dataStore.DeleteItemAsync(newItem.Id!))
                    {
                        Items.Remove(newItem);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    await alertService.DisplayAlertOkAsync(string.Empty, ex.Message);
                }
            }
        }

        private async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await dataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (ApiException ex) when (ex.Message.Contains("401"))
            {

                await navigationService.PushAsync<LoginViewModel>(new LoginViewModelArgs()
                {
                    HasSessionExpired = true,
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await alertService.DisplayAlertOkAsync(string.Empty, ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
