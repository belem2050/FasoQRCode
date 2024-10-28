using FasoQRCode.Views;

namespace FasoQRCode
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new NavigationPage(new MainPage());
            //MainPage = new NavigationPage(new MainPage());
            MainPage = new AppShell();

        }
    }
}
