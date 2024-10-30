using FasoQRCode.Views;
using FasoQRCode.ViewsModels;
using Microsoft.Extensions.Logging;
using ZXing.Net.Maui.Controls;

using CommunityToolkit.Maui;

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

            builder.Services.AddTransient<ResultPage>();

            builder.Services.AddTransient<PageSettings>();
            builder.Services.AddTransient<SettingsVM>();

            builder.Services.AddTransient<PageHistory>();
            return builder.Build();
        }
    }
}
