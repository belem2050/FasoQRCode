using CommunityToolkit.Mvvm.Input;
using FasoQRCode.Models.Data;

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

            if (first is null || !barcodeReader.IsDetecting)
            {
                return;
            }

            await Dispatcher.DispatchAsync(async () =>
            {
                barcodeReader.IsDetecting = false;
               
                try
                {

                    string qrThumbnailfilePath = await CaptureAndSaveQrImageAsync();
                    if (Manager.Settings.IsSoundEnabled)
                    {
                        soundPlayer.Stop();
                        soundPlayer.Play();
                    }
                    string encodedResult = Uri.EscapeDataString(first.Value);
                    Manager.CurrentHistoryItem = new HistoryItem
                    {
                        Date = DateTime.Now,
                        Content = first.Value,
                        QrThumbnail = qrThumbnailfilePath
                    };

                    await Shell.Current.GoToAsync($"//{nameof(MainPage)}/ResultPage?resultText={encodedResult}");
                }
                catch (Exception ex)
                {
                    ///
                }
            });
        }

        public async Task<string> CaptureAndSaveQrImageAsync()
        {
            try
            {
                string fileName = $"QR_{DateTime.Now:yyyyMMdd_HHmmss}.jpg";
                string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
                using var fileStream = File.Create(filePath);
                var capture = await barcodeReader.CaptureAsync();
                await capture.CopyToAsync(fileStream);

                return filePath;
            }
            catch
            {
                return string.Empty;
            }
        }
        [RelayCommand]
        public async Task ScanbyImage()
        {
            soundPlayer.Stop();
            soundPlayer.Play();
        }

        [RelayCommand]
        public void ToggleTorch()
        {
            Manager.Settings.IsTorchOn = !Manager.Settings.IsTorchOn;
        }

        [RelayCommand]
        public void SwapCamera()
        {
            try
            {

                if (Manager.Settings.DefaultCamera == ZXing.Net.Maui.CameraLocation.Rear)
                {
                    Manager.Settings.DefaultCamera = ZXing.Net.Maui.CameraLocation.Front;
                }
                else
                {
                    Manager.Settings.DefaultCamera = ZXing.Net.Maui.CameraLocation.Rear;
                }
            }
            catch (Exception ex)
            {
                //failure
            }
        }

    protected override void OnAppearing()
        {
            base.OnAppearing();
            barcodeReader.IsDetecting = true;
        }

        protected override void OnDisappearing()
        {
            barcodeReader.IsDetecting = false;
            base.OnDisappearing();
        }
    }


}


    