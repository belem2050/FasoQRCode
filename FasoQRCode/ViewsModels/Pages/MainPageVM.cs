using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FasoQRCode.Models.Data;
using ZXing.Net.Maui.Controls;

namespace FasoQRCode
{
    public partial class MainPageVM : ObservableObject
    {
        public SystemManager Manager { get; private set; } = SystemManager.GetInstance();

        private CameraBarcodeReaderView _barcodeReader;
        private MediaElement _soundPlayer;

        public MainPageVM(CameraBarcodeReaderView barcodeReader, MediaElement soundPlayer)
        {
            _barcodeReader = barcodeReader;
            _soundPlayer = soundPlayer;
            _barcodeReader.Options = new ZXing.Net.Maui.BarcodeReaderOptions
            {
                Formats = ZXing.Net.Maui.BarcodeFormat.QrCode,
                AutoRotate = Manager.Settings.IsAutoFocusEnabled,
                Multiple = true,
                TryHarder = true,
                
            };
            _barcodeReader.BarcodesDetected += _barcodeReader_BarcodesDetected;
        }

        private async void _barcodeReader_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
        {
            var first = e.Results?.FirstOrDefault();

            if (first is null || !_barcodeReader.IsDetecting)
            {
                return;
            }
            _barcodeReader.BarcodesDetected -= _barcodeReader_BarcodesDetected;

            await App.Current.MainPage.Dispatcher.DispatchAsync(async () =>
            {
                _barcodeReader.IsDetecting = false;

                try
                {

                    string qrThumbnailfilePath = await CaptureAndSaveQrImageAsync();
                    if (Manager.Settings.IsSoundEnabled)
                    {
                        _soundPlayer.Stop();
                        _soundPlayer.Play();
                    }

                    if (Manager.Settings.IsVibrationEnabled)
                    {
                        Vibration.Cancel();
                        Vibration.Default.Vibrate();
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
                var capture = await _barcodeReader.CaptureAsync();
                await capture.CopyToAsync(fileStream);

                return filePath;
            }
            catch
            {
                return string.Empty;
            }
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

}}


