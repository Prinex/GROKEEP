namespace Grokeep.Views;

public partial class GroceryInventoriesPage : ContentPage
{
	public GroceryInventoriesPage(GroceryInventoriesPageViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}