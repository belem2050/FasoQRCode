using CommunityToolkit.Mvvm.Input;
using FasoQRCode.Views;

namespace FasoQRCode
{
    public partial class MainPage : ContentPage
    {
        public SystemManager Manager { get; private set; } = SystemManager.GetInstance();

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;

            barcodeReader.Options = new ZXing.Net.Maui.BarcodeReaderOptions
            {
                Formats = ZXing.Net.Maui.BarcodeFormat.QrCode,
                AutoRotate = true,
                Multiple = true,
                TryHarder = true,
            };
        }

        private async void barcodeReader_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
        {
            var first = e.Results?.FirstOrDefault();

            if (first is null)
            {
                return;
            }

            Dispatcher.DispatchAsync(async () =>
            {
                try
                {
                    var encodedResult = Uri.EscapeDataString(first.Value);
                    await Shell.Current.GoToAsync($"{nameof(ResultPage)}?resultText={encodedResult}").ConfigureAwait(true);
                }
                catch (Exception ex)
                {
                    //Debug.WriteLine($"Navigation error: {ex.Message}");
                }
            });
        }

        [RelayCommand]
        public async void ScanbyImage()
        {

        }

        [RelayCommand]
        public async void ToggleTorch()
        {

            Manager.Settings.IsTorchon = !Manager.Settings.IsTorchon;
        }

        [RelayCommand]
        public void SwapCamera()
        {
            if(Manager.Settings.DefaultCamera  == ZXing.Net.Maui.CameraLocation.Rear)
            {
                Manager.Settings.DefaultCamera = ZXing.Net.Maui.CameraLocation.Front;
            }
            else
            {
                Manager.Settings.DefaultCamera = ZXing.Net.Maui.CameraLocation.Rear;
            }
        }
    }


}


    