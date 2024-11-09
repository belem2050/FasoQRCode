using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FasoQRCode.Models.Data;
using System.Net.Mail;
using ZXing.Net.Maui.Controls;

#if ANDROID
using Android.Content;
using Android.Provider;
using Android.Graphics;
using Android.Net;
#endif


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
        public async Task SaveQrCodeInGallery()
        {
            try
            {
                (string fileName, string filePath) = await GenerateAndSaveQrCodeImageInHistory();

                await SaveImageToGalleryAsync(filePath, fileName);
            }
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Can't save", "OK");
            }
        }


public async Task SaveImageToGalleryAsync(string filePath, string fileName)
    {
#if ANDROID
    try
    {
        // Ensure we have a valid bitmap (replace this with your actual file path)
        Bitmap bitmap = BitmapFactory.DecodeFile(filePath);

        // Prepare MediaStore values
        var contentValues = new ContentValues();
        contentValues.Put(MediaStore.Images.Media.InterfaceConsts.DisplayName, fileName);
        contentValues.Put(MediaStore.Images.Media.InterfaceConsts.MimeType, "image/jpeg");
        contentValues.Put(MediaStore.Images.Media.InterfaceConsts.RelativePath, "Pictures/FasoQRCode");

        // Insert the image using ContentResolver
        var uri = Android.App.Application.Context.ContentResolver.Insert(MediaStore.Images.Media.ExternalContentUri, contentValues);
        if (uri != null)
        {
            using (var outputStream = Android.App.Application.Context.ContentResolver.OpenOutputStream(uri))
            {
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, outputStream);
            }
        }

        // Update the UI to show success
        SaveIcon = ImageSource.FromFile("accept.png"); // Update SaveIcon to indicate success
    }
    catch (Exception ex)
    {
        // Handle any exceptions (e.g., logging)
        Console.WriteLine($"Error saving image to gallery: {ex.Message}");
    }
#else
        // For other platforms (Windows, iOS)
        var mediaPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), fileName);
        File.Copy(filePath, mediaPath, true);
        SaveIcon = ImageSource.FromFile("accept.png");
#endif
    }


    [RelayCommand(CanExecute = nameof(CanSaveOrShare))]
        public async Task ShareQrCode()
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

        private async Task<(string fileName, string filePath)> GenerateAndSaveQrCodeImageInHistory()
        {
            string fileName = $"QR_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            string filePath = System.IO.Path.Combine(FileSystem.AppDataDirectory, fileName);
            using var fileStream = File.Create(filePath);

            string ContenToSave = QrContent;

            if (MailAddress.TryCreate(QrContent, out var address))
            {
                QrContent = "mailto:" + address.Address;
            }
            var qrCodeImage = await _barcodeGenerator.CaptureAsync();

            await qrCodeImage.CopyToAsync(fileStream);
    
            Manager.HistoryItems.Add(new HistoryItem
            {
                Date = DateTime.Now,
                Content = ContenToSave,
                QrThumbnail = filePath
            });

            _historyService.SaveHistory(Manager.HistoryItems);

            return (fileName, filePath);
        }
    }
}
