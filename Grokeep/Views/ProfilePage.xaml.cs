namespace Grokeep.Views;

public partial class ProfilePage : ContentPage
{
	private ProfilePageViewModel _vm;
	public ProfilePage(ProfilePageViewModel vm)
	{
		InitializeComponent();
		_vm = vm;
		this.BindingContext = vm;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (App.UserSessionData != null)
        {
            _vm.AccountUsername = App.UserSessionData.Username;
            _vm.AccountPassword = App.UserSessionData.Password;
        }

    }
}