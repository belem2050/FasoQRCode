using CommunityToolkit.Mvvm.Input;

namespace FasoQRCode.Views
{
    [QueryProperty(nameof(ResultText), "resultText")]
    public partial class ResultPage : ContentPage
    {
        private readonly HistoryService _historyService = new HistoryService();

        public SystemManager Manager { get; private set; } = SystemManager.GetInstance();
        private string _resultText;
        public string ResultText
        {
            get => _resultText;
            set
            {
                _resultText = value;
                OnPropertyChanged();
            }
        }

        public ResultPage()
        {
            InitializeComponent();
            BindingContext = this;
            Manager.HistoryItems.Add(Manager.CurrentHistoryItem);

            _historyService.SaveHistory(Manager.HistoryItems);
            Manager.HistoryItems = _historyService.LoadHistory();
        }

        [RelayCommand]
        public async Task Copy()
        {
            await Clipboard.SetTextAsync(ResultText);
            await DisplayAlert("Copied", "Text copied to clipboard.", "OK");
        }

        [RelayCommand]
        public async Task OpenLink()
        {
            if (Uri.TryCreate(ResultText, UriKind.Absolute, out var uri))
            {
                await Launcher.Default.OpenAsync(uri);
            }
            else
            {
                await DisplayAlert("Invalid URL", "The scanned result is not a valid URL.", "OK");
            }
        }

        [RelayCommand]
        public async Task ShareResult()
        {
            if (string.IsNullOrEmpty(ResultText))
            {
                await DisplayAlert("Error", "Nothing to share.", "OK");
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
