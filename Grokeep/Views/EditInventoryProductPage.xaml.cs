namespace Grokeep.Views;

public partial class EditInventoryProductPage : ContentPage
{
	public EditInventoryProductPage(EditInventoryProductPageViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}