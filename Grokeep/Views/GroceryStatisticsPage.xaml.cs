namespace Grokeep.Views;

public partial class GroceryStatisticsPage : ContentPage
{
	public GroceryStatisticsPage(GroceryStatisticsPageViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}