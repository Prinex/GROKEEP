namespace Grokeep.Views;

public partial class MainPage : ContentPage
{
    private MainPageViewModel _vm;
	public MainPage(MainPageViewModel vm)
	{
		InitializeComponent();
        _vm = vm;
        this.BindingContext= vm;
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.DisplayAccountUsername();
    }
}

