using FasoQRCode.ViewModels.Pages;
using ZXing;

namespace FasoQRCode.Views;

public partial class PageCreateQR : ContentPage
{
	public PageCreateQR()
	{
		InitializeComponent();
		BindingContext = new PageCreateQrVM(barcodeGenerator);
	}
}