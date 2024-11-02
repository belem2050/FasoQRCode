using FasoQRCode.ViewModels;

namespace FasoQRCode.Views
{
    public partial class ResultPage : ContentPage
    {
        public ResultPage()
        {
            InitializeComponent();
           
            BindingContext = new PageResultVM();
        }
    }
}