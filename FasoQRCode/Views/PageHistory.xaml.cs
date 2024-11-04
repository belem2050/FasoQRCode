using FasoQRCode.ViewModels;

namespace FasoQRCode.Views;

public partial class PageHistory : ContentPage
{
	public PageHistory(PageHistoryVM vm)
	{
		InitializeComponent();
        BindingContext = vm;
	}
}