using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ZXing.Net.Maui;

namespace FasoQRCode.ViewsModels
{
    public partial class SettingsVM : ObservableObject
    {
        [ObservableProperty] 
        private bool isDarkModeEnabled;
        
        [ObservableProperty] 
        private bool isVibrationEnabled;
        
        [ObservableProperty] 
        private bool isSoundEnabled;

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
        private string camera;

  

        [ObservableProperty]
        private bool isAutoFocusEnabled;
        
        [ObservableProperty]
        private bool isTorchon;

        

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
            IsTorchon = false;
        }
    }
}
