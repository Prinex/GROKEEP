namespace Grokeep.Views;

public partial class AddInventoryProductPage : ContentPage
{
	public AddInventoryProductPage(AddInventoryProductPageViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}