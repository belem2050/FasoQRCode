using FasoQRCode.ViewModels.Pages;

namespace FasoQRCode.Views;

public partial class PageCreateQR : ContentPage
{
	public PageCreateQR()
	{
		InitializeComponent();
		BindingContext = new PageCreateQrVM(barcodeGenerator);
	}
}