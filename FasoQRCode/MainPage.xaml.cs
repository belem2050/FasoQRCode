using ZXing.Net.Maui.Controls;

namespace FasoQRCode
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageVM(barcodeReader, soundPlayer);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            barcodeReader = new CameraBarcodeReaderView();
            barcodeReader.IsDetecting = true;
        }

        protected override void OnDisappearing()
        {
            barcodeReader.IsDetecting = false;
            base.OnDisappearing();
        }
    }
}


    