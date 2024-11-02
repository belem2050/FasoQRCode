using CommunityToolkit.Mvvm.Input;
using FasoQRCode.Views;

namespace FasoQRCode
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            BindingContext =this;
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(ResultPage), typeof(ResultPage));
        }

        [RelayCommand]
        public async Task ShareApp()
        {
            string App = "Moumouni BELEM, the engineer that developped Faso QR code Reader, personal website\n https://belem2050.github.io/";
     

            var shareTextRequest = new Microsoft.Maui.ApplicationModel.DataTransfer.ShareTextRequest
            {
                Text = App,
                Title = "Share Faso QR code through platform"
            };

            await Share.Default.RequestAsync(shareTextRequest);
        }
    }
}
