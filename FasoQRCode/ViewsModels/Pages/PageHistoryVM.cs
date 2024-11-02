using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FasoQRCode.Models.Data;

namespace FasoQRCode.ViewModels
{
    public partial class PageHistoryVM : ObservableObject
    {

        public SystemManager Manager { get; private set; } = SystemManager.GetInstance();
        private readonly HistoryService _historyService = new HistoryService();

        public PageHistoryVM()
        {
            Manager.HistoryItems = _historyService.LoadHistory();
        }

        [RelayCommand]
        public void Delete(HistoryItem item)
        {
            if (item != null)
            {
                Manager.HistoryItems.Remove(item);
                File.Delete(item.QrThumbnail);
                _historyService.SaveHistory(Manager.HistoryItems);
            }
        }

        [RelayCommand]
        public void ClearHistory()
        {
            foreach (var item in Manager.HistoryItems)
            {
                File.Delete(item.QrThumbnail);
            }
            Manager.HistoryItems.Clear();
            _historyService.SaveHistory(Manager.HistoryItems);
        }
    }
}
