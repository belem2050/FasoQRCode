using FasoQRCode.ViewModels;

namespace FasoQRCode.Views
{
    public partial class ResultPage : ContentPage
    {
        public ResultPage(MainPageVM vm)
        {
            InitializeComponent();
           
            BindingContext = vm;
        }
    }
}