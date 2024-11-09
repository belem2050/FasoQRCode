using FasoQRCode.ViewModels;

namespace FasoQRCode.Views
{
    public partial class ResultPage : ContentPage
    {
        public ResultPage(PageResultVM vm)
        {
            InitializeComponent();
           
            BindingContext = vm;
        }
    }
}