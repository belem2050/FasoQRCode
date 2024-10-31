using CommunityToolkit.Mvvm.ComponentModel;
using FasoQRCode.Models;
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
        public ObservableCollection<HistoryItem> HistoryItems { get; set; } = new ObservableCollection<HistoryItem>();

        private SystemManager()
        {
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