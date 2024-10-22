﻿using CommunityToolkit.Mvvm.Input;

namespace FasoQRCode
{
    public partial class MainPage : ContentPage
    {

        public double ZoomRate
        {
            get
            {
                return _zoomRate;
            }
            set
            {
                _zoomRate = value;
                OnPropertyChanged();

            }
        }

        private double _zoomRate = 1;
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

        private void barcodeReader_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
        {
            var first = e.Results?.FirstOrDefault();

            if (first is null)
            {
                return;
            }

            Dispatcher.DispatchAsync(async () =>
            {
                await DisplayAlert("Barcode Detected", first.Value, "Burkina Faso is getting there");
            });
        }

        [RelayCommand]
        public void ScanbyImage()
        {

        }

        [RelayCommand]
        public void ToggleTorch()
        {
            barcodeReader.IsTorchOn = !barcodeReader.IsTorchOn;
        }

        [RelayCommand]
        public void SwapCamera()
        {
            if(barcodeReader.CameraLocation  == ZXing.Net.Maui.CameraLocation.Rear)
            {
                barcodeReader.CameraLocation = ZXing.Net.Maui.CameraLocation.Front;
            }
            else
            {
                barcodeReader.CameraLocation = ZXing.Net.Maui.CameraLocation.Rear;
            }
        }
    }


}


    