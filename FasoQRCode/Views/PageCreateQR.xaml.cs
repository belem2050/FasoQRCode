using FasoQRCode.ViewModels.Pages;

namespace FasoQRCode.Views;

public partial class PageCreateQR : ContentPage
{
	public PageCreateQR(PageCreateQrVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}