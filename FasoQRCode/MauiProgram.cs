using CommunityToolkit.Maui;
using FasoQRCode.ViewModels;
using FasoQRCode.ViewModels.Pages;
using FasoQRCode.Views;
using FasoQRCode.ViewsModels;
using FasoQRCode.ViewsModels.Pages;
using Microsoft.Extensions.Logging;
using ZXing.Net.Maui.Controls;

namespace FasoQRCode
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseBarcodeReader()
                .UseMauiCommunityToolkitMediaElement();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainPageVM>();

            builder.Services.AddTransient<ResultPage>();
            builder.Services.AddTransient<PageResultVM>();

            builder.Services.AddTransient<PageSettings>();
            builder.Services.AddTransient<PageSettingsVM>();

            builder.Services.AddTransient<PageHistory>();
            builder.Services.AddTransient<PageHistoryVM>();

            builder.Services.AddTransient<PageAboutBurkinaFaso>();
            builder.Services.AddTransient<PageAboutBurkinaFasoVM>(); 

            builder.Services.AddTransient<PageCreateQR>();
            builder.Services.AddTransient<PageCreateQrVM>(); 
            return builder.Build();
        }
    }
}
