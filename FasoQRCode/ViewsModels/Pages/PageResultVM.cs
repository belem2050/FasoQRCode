using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FasoQRCode.ViewModels
{
    [QueryProperty(nameof(ResultText), "resultText")]
    public partial class PageResultVM : ObservableObject
    {
        private readonly HistoryService _historyService = new HistoryService();

        public SystemManager Manager { get; private set; } = SystemManager.GetInstance();

        [ObservableProperty]
        private string resultText;

        [ObservableProperty]
        private ImageSource saveIcon = ImageSource.FromFile("save.png");

        [ObservableProperty]
        private ImageSource openLinkIcon = ImageSource.FromFile("open.png");

        [ObservableProperty]
        private ImageSource shareResultcon = ImageSource.FromFile("send.png");
 

        public PageResultVM()
        {
            Manager.HistoryItems.Add(Manager.CurrentHistoryItem);
            _historyService.SaveHistory(Manager.HistoryItems);
            Manager.HistoryItems = _historyService.LoadHistory();
        }

    [RelayCommand]
        public async Task Copy()
        {
            await Clipboard.SetTextAsync(ResultText);
            await App.Current.MainPage.DisplayAlert("Copied", "Text copied to clipboard.", "OK");
        }

        [RelayCommand]
        public async Task OpenLink()
        {
            if (Uri.TryCreate(ResultText, UriKind.Absolute, out var uri))
            {
                await Launcher.Default.OpenAsync(uri);
                SaveIcon = ImageSource.FromFile("check.png");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Invalid URL", "The scanned result is not a valid URL.", "OK");
            }
        }

        [RelayCommand]
        public async Task ShareResult()
        {
            if (string.IsNullOrEmpty(ResultText))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Nothing to share.", "OK");
                return;
            }

            var shareTextRequest = new ShareTextRequest
            {
                Text = ResultText,
                Title = "Share Scanned Result"
            };

            await Share.Default.RequestAsync(shareTextRequest);
        }
    }
}
