namespace FasoQRCode
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageVM(barcodeReader, soundPlayer/*, cameraView*/);
        }

        protected override void OnAppearing()
        {
            barcodeReader.IsDetecting = true;
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            barcodeReader.IsDetecting = false;
            base.OnDisappearing();
        }
    }
}