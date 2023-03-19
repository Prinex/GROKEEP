namespace Grokeep.Views;

public partial class ForgotUserPasswordPage : ContentPage
{
	public ForgotUserPasswordPage(ForgotUserPasswordPageViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}