namespace Grokeep.Views;

public partial class GroceryStatisticsPage : ContentPage
{
	private GroceryStatisticsPageViewModel _vm;
    public GroceryStatisticsPage(GroceryStatisticsPageViewModel vm)
	{
		InitializeComponent();
		_vm = vm;
		this.BindingContext = vm;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.InitChart();
    }

}