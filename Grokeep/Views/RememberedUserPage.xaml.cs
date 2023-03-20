namespace Grokeep.Views;

public partial class RememberedUserPage : ContentPage
{
	public RememberedUserPage(RememberedUserPageViewModel vm)
	{
		InitializeComponent();

        this.BindingContext = vm;
	}
}