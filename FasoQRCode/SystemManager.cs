using CommunityToolkit.Mvvm.ComponentModel;
using FasoQRCode.Models.Data;
using FasoQRCode.ViewsModels;
using System.Collections.ObjectModel;
using ZXing.Net.Maui;

namespace FasoQRCode
{
    public sealed class SystemManager : ObservableObject
    {
        private static object _lockInstance = new object();
        static private SystemManager? _instance = null;

        public PageSettingsVM Settings { get; set; } = new PageSettingsVM();

        public HistoryItem CurrentHistoryItem { get; set; } = new HistoryItem();
        public ObservableCollection<HistoryItem> HistoryItems { get; set; } = new ObservableCollection<HistoryItem>();

        private readonly HistoryService _historyService = new HistoryService();

        private SystemManager()
        {
            _instance = this;
            HistoryItems = _historyService.LoadHistory();
            _historyService.SaveHistory(HistoryItems);

            Settings.PropertyChanged += Settings_PropertyChanged;
        }

        private void Settings_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(PageSettingsVM.IsDarkModeEnabled))
            {
                Application.Current.UserAppTheme = Settings.IsDarkModeEnabled ? AppTheme.Dark : AppTheme.Light;
            }

            if (e.PropertyName == nameof(PageSettingsVM.Camera))
            {
                Settings.DefaultCamera = (Settings.Camera == "Rear") ? CameraLocation.Rear : CameraLocation.Front;
            }

            if (e.PropertyName == nameof(PageSettingsVM.IsTorchOn))
            {
              Settings.FlashIcon = Settings.IsTorchOn ? ImageSource.FromFile("flash.png") : ImageSource.FromFile("flash_off.png");
            }

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