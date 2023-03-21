namespace Grokeep.ViewModels;
public partial class LoginPageViewModel : BaseViewModel
{
    private readonly IUserService userService;

    [ObservableProperty]
    public bool keepUsrLoggedIn = false;

    [ObservableProperty]
    private User userCredentials = new User();

    public LoginPageViewModel(IUserService userService)
    {
        this.userService = userService;
    }

    [RelayCommand]
    public async void Login()
    {
        if (IsBusy == true)
            return;

        // used to indicate current process with activity indicator in view
        IsBusy = true;

        User usrCredentials = new User();

        if (string.IsNullOrEmpty(UserCredentials.Username) || string.IsNullOrEmpty(UserCredentials.Password))
        {
            IsBusy = false;
            await Shell.Current.DisplayAlert("Warning", "Provide both username and password for creating your user account.", "OK");
        }
        else if (!string.IsNullOrEmpty(UserCredentials.Username) && !string.IsNullOrEmpty(UserCredentials.Password))
        {
            usrCredentials = await userService.RetrieveUser(UserCredentials.Username);

            if (usrCredentials.DoesExist != true)
            {
                IsBusy = false;
                await Shell.Current.DisplayAlert("Warning", $"Account {UserCredentials.Username} does not exist.", "OK");
            }
            else if (usrCredentials.DoesExist == true)
            {
                
                // verify the pwd if account exists
                var encoder = new ScryptEncoder();
                if (encoder.Compare(UserCredentials.Password, usrCredentials.Password) == true)
                {
                    // make use of Preferences to save data related to the user for later usage, e.g. credentials
                    // at each login time delete previous user credentials and save the possible new user credentials
                    if (Preferences.ContainsKey(nameof(App.UserSessionData)) && Preferences.ContainsKey(nameof(KeepUsrLoggedIn)))
                    {
                        Preferences.Remove(nameof(KeepUsrLoggedIn));
                        Preferences.Remove(nameof(App.UserSessionData));
                    }
                    // if user checked the keep me logged in box will not require login next time
                    if (KeepUsrLoggedIn == true)
                    {
                        Preferences.Set(nameof(KeepUsrLoggedIn), KeepUsrLoggedIn);
                    }

                    // saving the credentials
                    // serialize the user credentials object instance
                    usrCredentials.Password = UserCredentials.Password; 
                    string userCredentialsSerialized = JsonConvert.SerializeObject(usrCredentials);
                    Preferences.Set(nameof(App.UserSessionData), userCredentialsSerialized);
                    App.UserSessionData = usrCredentials;

                    IsBusy = false;
                    // goto main page
                    await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                    
                }
                else
                {
                    IsBusy = false;
                    await Shell.Current.DisplayAlert("Login error", "The inserted password is wrong.", "OK");
                }
            }
        }
    }

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

