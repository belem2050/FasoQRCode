using CommunityToolkit.Mvvm.ComponentModel;
using FasoQRCode.Models.Data;
using FasoQRCode.ViewsModels;
using System.Collections.ObjectModel;
using ZXing;

namespace FasoQRCode
{
    public sealed class SystemManager : ObservableObject
    {
        private static object _lockInstance = new object();
        static private SystemManager? _instance = null;

        public SettingsVM Settings { get; set; } = new SettingsVM();
        public HistoryItem CurrentHistoryItem { get; set; } = new HistoryItem();
        public ObservableCollection<HistoryItem> HistoryItems { get; set; } = new ObservableCollection<HistoryItem>();

        private readonly HistoryService _historyService = new HistoryService();


        private SystemManager()
        {
            HistoryItems = _historyService.LoadHistory();
            _historyService.SaveHistory(HistoryItems);
        }

        static public SystemManager GetInstance()
        {
            lock (_lockInstance)
            {
                if (_instance is null)
                {
                    return _instance = new SystemManager();
                }
                return _instance;
            }
        }



    }
}