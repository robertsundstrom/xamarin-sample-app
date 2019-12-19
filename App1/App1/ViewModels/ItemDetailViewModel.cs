﻿
using App1.MobileAppService.Client;

namespace App1.ViewModels
{
    public class ItemDetailViewModel : ViewModelBase
    {
        public Item? Item { get; set; }
        public ItemDetailViewModel(Item? item = null)
        {
            Title = item?.Text;
            Item = item;
        }
    }
}
