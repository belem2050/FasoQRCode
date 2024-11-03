using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ZXing.Net.Maui;

namespace FasoQRCode.ViewsModels
{
    public partial class PageSettingsVM : ObservableObject
    {
        [ObservableProperty] 
        private bool isVibrationEnabled;
        
        [ObservableProperty] 
        private bool isSoundEnabled = true;

        [ObservableProperty]
        private CameraLocation defaultCamera = CameraLocation.Rear;

        [ObservableProperty]
        private string camera = "Rear";

        [ObservableProperty]
        private bool isAutoFocusEnabled;

        [ObservableProperty]
        private bool isTorchOn;

        [ObservableProperty]
        public ImageSource flashIcon = ImageSource.FromFile("flash_off.png");

        [ObservableProperty]
        private bool isDarkModeEnabled = (Application.Current.UserAppTheme == AppTheme.Dark);

        public PageSettingsVM()
        {
        }

        [RelayCommand]
        public void ResetSettings()
        {
            IsDarkModeEnabled = false;
            IsVibrationEnabled = true;
            IsSoundEnabled = true;
            DefaultCamera = CameraLocation.Rear;
            IsAutoFocusEnabled = true;
            IsTorchOn = false;
        }
    }
}
