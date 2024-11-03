using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FasoQRCode.Models.Data;
using SkiaSharp;
using ZXing;
using ZXing.Net.Maui.Controls;

namespace FasoQRCode
{
    public partial class MainPageVM : ObservableObject
    {
        public SystemManager Manager { get; private set; } = SystemManager.GetInstance();

        private CameraBarcodeReaderView _barcodeReader;
        private MediaElement _soundPlayer;
        //private CommunityToolkit.Maui.Views.CameraView _cameraView;

        [ObservableProperty]
        private Visibility barcodeReaderVisiblity = Visibility.Visible;
        
        [ObservableProperty]
        private Visibility cameraVisiblity = Visibility.Collapsed;

        [ObservableProperty]
        private double zoomRate; 

        [ObservableProperty]
        private double sliderValue;

        public MainPageVM(CameraBarcodeReaderView barcodeReader, MediaElement soundPlayer/*, CommunityToolkit.Maui.Views.CameraView cameraView*/)
        {
            _barcodeReader = barcodeReader;
            _soundPlayer = soundPlayer;
            BarcodeReaderVisiblity = Visibility.Visible;
            _barcodeReader.Options = new ZXing.Net.Maui.BarcodeReaderOptions
            {
                Formats = ZXing.Net.Maui.BarcodeFormat.QrCode,
                AutoRotate = Manager.Settings.IsAutoFocusEnabled,
                Multiple = true,
                TryHarder = true,
                
            };
            _barcodeReader.BarcodesDetected += _barcodeReader_BarcodesDetected;
            this.PropertyChanging += MainPageVM_PropertyChanging;
            this.PropertyChanged += MainPageVM_PropertyChanged;


            //if(_cameraView.SelectedCamera != null)
            //{
            //    ZoomRate = _cameraView.SelectedCamera.MinimumZoomFactor;
            //    sliderValue = ZoomRate;
            //}
        }

        private void MainPageVM_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.ZoomRate))
            {
                CameraVisiblity = Visibility.Collapsed;
                BarcodeReaderVisiblity = Visibility.Visible;
            }

            //if (e.PropertyName == nameof(this.SliderValue) && _cameraView.SelectedCamera != null)
            //{
            //    if(SliderValue < _cameraView.SelectedCamera.MinimumZoomFactor)
            //    {
            //        ZoomRate = _cameraView.SelectedCamera.MinimumZoomFactor;
            //    }

            //    if (SliderValue > _cameraView.SelectedCamera.MaximumZoomFactor)
            //    {
            //        ZoomRate = _cameraView.SelectedCamera.MaximumZoomFactor;
            //    }
            //}
        }

        private void MainPageVM_PropertyChanging(object? sender, System.ComponentModel.PropertyChangingEventArgs e)
        {
            if (e.PropertyName == nameof(this.ZoomRate))
            {
                CameraVisiblity = Visibility.Visible;
                BarcodeReaderVisiblity = Visibility.Collapsed;
            }
        }

        private async void _barcodeReader_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
        {
            var first = e.Results?.FirstOrDefault();

            if (first is null || !_barcodeReader.IsDetecting)
            {
                return;
            }

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

                    if (Vibration.Default.IsSupported)
                    {
                        if (Manager.Settings.IsVibrationEnabled)
                        {
                            Vibration.Cancel();
                            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(500));
                        }
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
        public async void ScanbyImage()
        {
            try
            {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                FileResult pickedFile = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select a QR Code Image",
                    FileTypes = FilePickerFileType.Images
                });
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

                if (pickedFile != null)
                {
                    Stream pickedFilestream = await pickedFile.OpenReadAsync();
                    using SKBitmap skiaBitmap = SKBitmap.Decode(pickedFilestream);
                    BarcodeReaderGeneric barcodeReader = new BarcodeReaderGeneric();
                    Result result = barcodeReader.Decode(skiaBitmap.Bytes, skiaBitmap.Width, skiaBitmap.Height, RGBLuminanceSource.BitmapFormat.Unknown);

                    await App.Current.MainPage.Dispatcher.DispatchAsync(async () =>
                    {
                        try
                        {
                            string filePath = Path.Combine(FileSystem.AppDataDirectory, pickedFile.FileName);
                            using Stream fileStream = File.Create(filePath);
                            Stream pickedFilestreamToCopy = await pickedFile.OpenReadAsync();
                            await pickedFilestreamToCopy.CopyToAsync(fileStream);

                            Manager.CurrentHistoryItem = new HistoryItem
                            {
                                Date = DateTime.Now,
                                Content = result.Text,
                                QrThumbnail = filePath
                            };
                            await Shell.Current.GoToAsync($"//{nameof(MainPage)}/ResultPage?resultText={result.Text}");
                        }
                        catch (Exception ex)
                        {
                            ///
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                //DecodedQrContent = $"Error: {ex.Message}";
            }

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

        [RelayCommand] 
        public void ZoomOut()
        {
             SliderValue -= 0.1f;
            //if(_cameraView.SelectedCamera != null)
            //{

                //ZoomRate = SliderValue > _cameraView.SelectedCamera.MinimumZoomFactor ? SliderValue : _cameraView.SelectedCamera.MinimumZoomFactor;
            //}
        }

        [RelayCommand] 
        public void ZoomIn()
        {
             SliderValue += 0.1f;
            //if (_cameraView.SelectedCamera != null)
            //{

                //ZoomRate = SliderValue < _cameraView.SelectedCamera.MaximumZoomFactor ? SliderValue : _cameraView.SelectedCamera.MaximumZoomFactor;
            //}
        }
    }
}


