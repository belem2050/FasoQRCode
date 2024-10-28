using CommunityToolkit.Mvvm.Input;

namespace FasoQRCode.Views
{
    [QueryProperty(nameof(ResultText), "resultText")]
    public partial class ResultPage : ContentPage
    {
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
        public async Task Share()
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

            //try
            //{
            //    await share.Default.RequestAsync(shareTextRequest);
            //}
            //catch (Exception ex)
            //{
            //    await DisplayAlert("Sharing Failed", ex.Message, "OK");
            //}
        }

    }
}
