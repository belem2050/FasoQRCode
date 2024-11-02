using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;

namespace FasoQRCode.ViewsModels.Pages
{
    public partial class PageAboutBurkinaFasoVM : ObservableObject
    {
        private MediaElement _videoPlayer;
        private string videoSourcePath;

        public PageAboutBurkinaFasoVM(MediaElement videoPlayer)
        {
            _videoPlayer = videoPlayer;
        }
    }
}