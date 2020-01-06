namespace App1.ViewModels
{
    public class ItemDetailViewModel : ViewModelBase
    {
        public Models.Item? Item { get; set; }
        public ItemDetailViewModel(Models.Item? item = null)
        {
            Title = item?.Text;
            Item = item;
        }
    }
}
