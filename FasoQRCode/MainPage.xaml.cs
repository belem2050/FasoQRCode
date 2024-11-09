namespace FasoQRCode
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageVM(barcodeReader);
        }
    }
}