namespace Grokeep.Views;

public partial class GroceryInventoriesPage : ContentPage
{
	private GroceryInventoriesPageViewModel _vm;
	public GroceryInventoriesPage(GroceryInventoriesPageViewModel vm)
	{
		InitializeComponent();
		_vm = vm;
		this.BindingContext = vm;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await _vm.GetInventories();
	}
}