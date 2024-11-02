using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FasoQRCode.Models.Data;
using ZXing.Net.Maui.Controls;

namespace FasoQRCode.ViewModels.Pages
{
    public partial class PageCreateQrVM : ObservableObject
    {
        public SystemManager Manager { get; private set; } = SystemManager.GetInstance();
        private readonly HistoryService _historyService = new HistoryService();
        private BarcodeGeneratorView _barcodeGenerator;
        
        [ObservableProperty]
        private int height;

        [ObservableProperty]
        private int width;


        [ObservableProperty]
        private ImageSource saveIcon = ImageSource.FromFile("save.png");

        [ObservableProperty]
        private ImageSource shareResultcon = ImageSource.FromFile("send.png");


        public string QrContent
        {
            get
            {
                return qrContent;
            }
            set
            {
                qrContent = value;
                OnPropertyChanged();
                SaveQrCodeInGalleryCommand.NotifyCanExecuteChanged();
                ShareQrCodeCommand.NotifyCanExecuteChanged();
                SaveIcon = ImageSource.FromFile("save.png");
            }
        }

        private string qrContent;

        public string QrSize
        {
            get
            {
                return qrSize;
            }
            set
            {
                qrSize = value;
                switch (value)
                {
                    case "Small":
                        Height = 150;
                        Width = 150;
                        break;
                                       
                    case "Medium":
                        Height = 250;
                        Width = 250;
                        break;  
                    
                    case "Large":
                        Height = 350;
                        Width = 350;
                        break;
                }
            }
        }
        private string qrSize = "Small";

        [ObservableProperty]
        private ImageSource qrCodeImage;

        public PageCreateQrVM(BarcodeGeneratorView barcodeGenerator)
        {
            _barcodeGenerator = barcodeGenerator;
            SaveIcon = ImageSource.FromFile("save.png");
        }

        [RelayCommand(CanExecute = nameof(CanSaveOrShare))]
        private async Task SaveQrCodeInGallery()
        {
            try
            {
                (string fileName, string filePath) = await GenerateAndSaveQrCodeImageInHistory();
             
                var mediaPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), fileName);
                File.Copy(filePath, mediaPath, true);

                SaveIcon = ImageSource.FromFile("accept.png");
            }
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Can't save", "OK");
            }
        }

        [RelayCommand(CanExecute = nameof(CanSaveOrShare))]
        private async Task ShareQrCode()
        {
            try
            {

                (string fileName, string filePath) = await GenerateAndSaveQrCodeImageInHistory();

                await Share.Default.RequestAsync(new ShareFileRequest
                {
                    Title = "Share QR Code",
                    File = new ShareFile(filePath)
                } );


            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"Failed to share QR code: {ex.Message}", "OK");
            }
        }

        private bool CanSaveOrShare()
        {
            if(!string.IsNullOrEmpty(QrContent))
            {
                return true;
            }
            return false;
        }

        public async Task<(string fileName, string filePath)> GenerateAndSaveQrCodeImageInHistory()
        {
            string fileName = $"QR_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
            using var fileStream = File.Create(filePath);
            var qrCodeImage = await _barcodeGenerator.CaptureAsync();

            await qrCodeImage.CopyToAsync(fileStream);

            Manager.HistoryItems.Add(new HistoryItem
            {
                Date = DateTime.Now,
                Content = QrContent,
                QrThumbnail = filePath
            });

            _historyService.SaveHistory(Manager.HistoryItems);

            return (fileName, filePath);
        }
    }
}
