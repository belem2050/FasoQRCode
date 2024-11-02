using FasoQRCode.Views;
using FasoQRCode.ViewsModels;
using Microsoft.Extensions.Logging;
using ZXing.Net.Maui.Controls;

using CommunityToolkit.Maui;
using FasoQRCode.ViewModels;
using FasoQRCode.ViewsModels.Pages;

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
            return builder.Build();
        }
    }
}
