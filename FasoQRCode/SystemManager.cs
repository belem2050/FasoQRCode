using CommunityToolkit.Mvvm.ComponentModel;
using FasoQRCode.ViewsModels;
using ZXing;

namespace FasoQRCode
{
    public sealed class SystemManager : ObservableObject
    {
        private static object _lockInstance = new object();
        static private SystemManager? _instance = null;

        public SettingsVM Settings { get; set; } = new SettingsVM();


        private SystemManager()
        {
        }

        static public SystemManager GetInstance()
        {
            lock (_lockInstance)
            {
                if (_instance is null)
                {
                    return _instance = new SystemManager();
                }
                return _instance;
            }
        }



    }
}