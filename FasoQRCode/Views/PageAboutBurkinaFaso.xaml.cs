using CommunityToolkit.Maui.Views;
using FasoQRCode.ViewsModels.Pages;

namespace FasoQRCode.Views;

public partial class PageAboutBurkinaFaso : ContentPage
{
	public PageAboutBurkinaFaso()
	{
		InitializeComponent();
		BindingContext = new PageAboutBurkinaFasoVM(videoPlayer);
	}
}