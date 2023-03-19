namespace Grokeep.ViewModels;
public partial class LoginPageViewModel : BaseViewModel
{
    [RelayCommand]
    public async void GoToRegisterPage()
    {
        await Shell.Current.GoToAsync($"/{nameof(RegisterPage)}");
    }

    [RelayCommand]
    public async void GoToForgotPasswordPage()
    {
        await Shell.Current.GoToAsync($"/{nameof(ForgotUserPasswordPage)}");
    }
}

