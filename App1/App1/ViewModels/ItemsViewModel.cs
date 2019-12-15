using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using App1.Models;
using App1.Services;
using App1.Views;

using Xamarin.Forms;

namespace App1.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private readonly IDataStore<Item> dataStore;
        private readonly INativeCalls nativeCalls;

        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel(IDataStore<Item> dataStore, INativeCalls nativeCalls)
        {
            Title = "Browse";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            this.dataStore = dataStore;
            this.nativeCalls = nativeCalls;
            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                try
                {
                    if (await dataStore.AddItemAsync(newItem))
                    {
                        Items.Add(newItem);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    nativeCalls.OpenToast(ex.Message);
                }
            });

            MessagingCenter.Subscribe<ItemsPage, Item>(this, "DeleteItem", async (obj, item) =>
            {
                await DeleteItem(dataStore, nativeCalls, item);
            });

            MessagingCenter.Subscribe<ItemDetailPage, Item>(this, "DeleteItem", async (obj, item) =>
            {
                await DeleteItem(dataStore, nativeCalls, item);
            });
        }

        private async Task DeleteItem(IDataStore<Item> dataStore, INativeCalls nativeCalls, Item item)
        {
            var newItem = item as Item;

            try
            {
                if (await dataStore.DeleteItemAsync(newItem.Id))
                {
                    Items.Remove(newItem);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                nativeCalls.OpenToast(ex.Message);
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
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                nativeCalls.OpenToast(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
