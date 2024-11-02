using FasoQRCode.ViewsModels;

namespace FasoQRCode.Views;

public partial class PageSettings : ContentPage
{
	public SystemManager Manager { get; private set; } = SystemManager.GetInstance();

    public PageSettings()
	{
		InitializeComponent();
		BindingContext = Manager.Settings;
	}
}