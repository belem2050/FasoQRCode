using CommunityToolkit.Mvvm.Input;
using FasoQRCode.Models;
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
            barcodeReader.IsDetecting = false;
            await Dispatcher.DispatchAsync(async () =>
            {
                try
                {
                    if (Manager.Settings.IsSoundEnabled)
                    {
                        soundPlayer.Stop();
                        soundPlayer.Play();
                    }

                    string encodedResult = Uri.EscapeDataString(first.Value);

                    Manager.HistoryItems.Add(new HistoryItem
                    {
                        Title = "New Scan",  
                        Date = DateTime.Now,
                        Content = first.Value,
                        QrThumbnail = "qr_placeholder.png"  
                    });

                    await Shell.Current.GoToAsync($"//{nameof(MainPage)}/ResultPage?resultText={encodedResult}");
                    //.ConfigureAwait(true);
                }
                catch (Exception ex)
                {
                    //Debug.WriteLine($"Navigation error: {ex.Message}");
                }
                finally
                {
                    barcodeReader.IsDetecting=true;
                }
            });
        }

        [RelayCommand]
        public void ScanbyImage()
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
            // Ensure the camera starts detecting when the page appears
            barcodeReader.IsDetecting = true;
        }

        protected override void OnDisappearing()
        {
            // Optionally stop detecting when leaving the page
            barcodeReader.IsDetecting = false;
            base.OnDisappearing();
        }
    }


}


    