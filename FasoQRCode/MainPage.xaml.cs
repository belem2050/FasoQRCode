
using ZXing.Net.Maui.Controls;

namespace FasoQRCode
{
    public partial class MainPage : ContentPage
    {

        public MainPage(MainPageVM vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}