using FasoQRCode.ViewModels;

namespace FasoQRCode.Views;

public partial class PageHistory : ContentPage
{
	public PageHistory()
	{
		InitializeComponent();
		BindingContext = new PageHistoryVM();
	}
}