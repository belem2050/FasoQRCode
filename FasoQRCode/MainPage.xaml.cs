using CommunityToolkit.Mvvm.Input;
using FasoQRCode.Views;

namespace FasoQRCode
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        [RelayCommand]
        public async Task GoToTab()
        {
            //await Navigation.PushAsync(new PageMainTabbed());
            //await Navigation.PushAsync(new FlyoutPage());
        }
    }

}
