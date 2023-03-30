namespace Grokeep.Views;

public partial class FilteringGroceryRecordsPage : ContentPage
{
	private FilteringGroceryRecordsPageViewModel _vm;
	public FilteringGroceryRecordsPage(FilteringGroceryRecordsPageViewModel vm)
	{
		InitializeComponent();
		_vm = vm;
		this.BindingContext = vm;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.GetProductsHistory();
    }
}