using FasoQRCode.Views;

namespace FasoQRCode
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(ResultPage), typeof(ResultPage));
        }
    }
}
