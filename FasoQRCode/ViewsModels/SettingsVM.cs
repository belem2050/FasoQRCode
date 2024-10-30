using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ZXing.Net.Maui;

namespace FasoQRCode.ViewsModels
{
    public partial class SettingsVM : ObservableObject
    {
        [ObservableProperty] 
        private bool isVibrationEnabled;
        
        [ObservableProperty] 
        private bool isSoundEnabled = true;

        [ObservableProperty]
        private CameraLocation defaultCamera;

        public string Camera
        {
            get
            {
                return camera;
            }
            set
            {
                camera = value;
                if(camera == "Rear")
                {
                    DefaultCamera = CameraLocation.Rear;
                }
                else
                {
                    DefaultCamera = CameraLocation.Front;
                }
            }
        }
        private string camera = "Rear";

  

        [ObservableProperty]
        private bool isAutoFocusEnabled;
        
        public bool IsTorchOn
        {
            get
            {
                return isTorchon;
            }
            set
            {
                isTorchon = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FlashIcon));

            }
        }
        private bool isTorchon;

        //[ObservableProperty]
        public ImageSource FlashIcon
        {
            get
            {
                return IsTorchOn ? ImageSource.FromFile("flash.png")
                                 : ImageSource.FromFile("flash_off.png");
            }
        }


        public bool IsDarkModeEnabled
        {
            get
            {
                return _isDarkModeEnabled;
            }
            set
            {
                _isDarkModeEnabled = value;
                Application.Current.UserAppTheme = value ? AppTheme.Dark : AppTheme.Light;
                OnPropertyChanged();
            }
        }
        private bool _isDarkModeEnabled;


        public async Task ClearCache()
        {
           
        }

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
