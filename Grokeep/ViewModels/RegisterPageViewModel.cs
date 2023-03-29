namespace Grokeep.ViewModels;

public partial class RegisterPageViewModel : BaseViewModel
{
    private readonly IUserService userService;

    [ObservableProperty]
    private User userCredentials = new User();

    [ObservableProperty]
    public bool privacyRead = false;

    [ObservableProperty]
    public bool termsRead = false;

    public RegisterPageViewModel(IUserService userService)
    {
        this.userService = userService;
    }

    [RelayCommand]
    public async void GoToPrivacyPage()
    {   
        await Shell.Current.GoToAsync($"/{nameof(RegisterPrivacyPolicyPage)}");
    }

    [RelayCommand]
    public async void GoToTermsConditionsPage()
    {
        await Shell.Current.GoToAsync($"/{nameof(RegisterTermsAndConditionsPage)}");
    }

    [RelayCommand]
    public async void Register()
    {
        bool createResponse = false;
        IsBusy = true;

        if (string.IsNullOrEmpty(UserCredentials.Username) || string.IsNullOrEmpty(UserCredentials.Password))
        {
            IsBusy = false;
            await Shell.Current.DisplayAlert("Warning", "Provide both username and password for creating your user account.", "OK");
        }
        else if (!string.IsNullOrEmpty(UserCredentials.Username) && !string.IsNullOrEmpty(UserCredentials.Password) && (TermsRead == false || PrivacyRead == false))
        {
            IsBusy = false;
            await Shell.Current.DisplayAlert("Warning", "You need to agree with terms and conditions and privacy policy to create your account.", "OK");
        }
        else if (!string.IsNullOrEmpty(UserCredentials.Username) && !string.IsNullOrEmpty(UserCredentials.Password))
        {
            // hash and salt the password when adding it to the dbs using scrypt hashing algorithm, a better choice than bcrypt today
            // the salting part is included in the library function as part of the process of hashing the pwd
            try
            {
                var encoder = new ScryptEncoder();
                createResponse = await userService.AppendUser(new User
                {
                    Username = UserCredentials.Username,
                    Password = encoder.Encode(UserCredentials.Password)
                });
            }
            catch
            {
                // usernames are unique in the dbs so catch the error
                // the output would quite long, so only doing more of a general catch clause
                await Shell.Current.DisplayAlert("Registration error", $"{UserCredentials.Username} username is used by another account.", "OK");
            }
            finally
            {
                IsBusy = false;
                if (createResponse == true)
                {
                    await Shell.Current.DisplayAlert("Succesfull operation", $"{UserCredentials.Username} account was successfully created.", "OK");
                }
            }
        }
    }
}

