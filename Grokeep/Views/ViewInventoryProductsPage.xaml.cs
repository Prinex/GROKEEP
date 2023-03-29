namespace Grokeep.Views;

public partial class ViewInventoryProductsPage : ContentPage
{
	private ViewInventoryProductsPageViewModel _vm;
	public ViewInventoryProductsPage(ViewInventoryProductsPageViewModel vm)
	{
		InitializeComponent();
		_vm = vm;
		this.BindingContext = vm;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await _vm.GetInventoryProducts();
	}
}