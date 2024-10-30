using CommunityToolkit.Mvvm.Input;
using FasoQRCode.ViewsModels;

namespace FasoQRCode.Views;

public partial class PageSettings : ContentPage
{
    public SystemManager Manager { get; private set; } = SystemManager.GetInstance();

	public PageSettings()
	{
		InitializeComponent();
        BindingContext = this;
    }


    [RelayCommand]
    public async Task ManagePermissions()
    {
        // Handle permissions logic
    }

    [RelayCommand]
    public async Task ClearCache()
    {
        // Handle cache clearing logic
    }

    [RelayCommand]
    public async Task ResetSettings()
    {
        Manager.Settings.ResetSettings();
    }


}