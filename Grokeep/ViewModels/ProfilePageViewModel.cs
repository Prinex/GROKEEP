namespace Grokeep.ViewModels;
public partial class ProfilePageViewModel : BaseViewModel
{
    private readonly IUserService userService;

    [ObservableProperty]
    string accountUsername;

    [ObservableProperty]
    string accountPassword;

    public ProfilePageViewModel(IUserService userService)
    {
        this.userService = userService;

        if (App.UserSessionData != null)
        {
            AccountUsername = App.UserSessionData.Username;
            AccountPassword = App.UserSessionData.Password;
        }
    }

    [RelayCommand]
    public async Task Modify()
    {
        if (IsBusy == true)
            return;
        
        IsBusy = true;

        var userCredentials = new User();
        bool modifyRequest = false;

        userCredentials = await userService.RetrieveUser(App.UserSessionData.Username);

        if (string.IsNullOrEmpty(AccountUsername) || string.IsNullOrEmpty(AccountPassword)) 
        {
            IsBusy = false;
            await Shell.Current.DisplayAlert("Warning", "Credential fields are empty.", "OK");
        }
        else if (!string.IsNullOrEmpty(AccountUsername) && !string.IsNullOrEmpty(AccountPassword))
        {
            var encoder = new ScryptEncoder();
            if (userCredentials.Username == AccountUsername && encoder.Compare(AccountPassword, userCredentials.Password) == true)
            {
                IsBusy = false;
                await Shell.Current.DisplayAlert("Warning", "Credentials fields have not been changed at all.", "OK");
            }
            else 
            {
                try
                {
                    var encoderNewPwd = new ScryptEncoder();
                    userCredentials.Username = AccountUsername;
                    userCredentials.Password = encoderNewPwd.Encode(AccountPassword);
                    modifyRequest = await userService.UpdateUser(userCredentials);
                    userCredentials.Password = AccountPassword;
                }
                catch
                {
                    await Shell.Current.DisplayAlert("Error", "Username already exists. Try a different combination of characters", "OK");
                }
                if (modifyRequest == true)
                {
                    
                    await Shell.Current.DisplayAlert("Update", "The account has been updated successfully with the new credentials.", "OK");

                    if (Preferences.ContainsKey(nameof(App.UserSessionData))) 
                    {
                        Preferences.Remove(nameof(App.UserSessionData));
                    }

                    string newUserSerializedSessionData = JsonConvert.SerializeObject(userCredentials);
                    Preferences.Set(nameof(App.UserSessionData), newUserSerializedSessionData);
                    App.UserSessionData = userCredentials;

                    IsBusy = false;
                }
            }
        }
    }

    [RelayCommand]
    public async Task Remove()
    {
        if (IsBusy == true) 
        {
            return;
        }
        var confirmationRemoveUser = await Shell.Current.DisplayActionSheet("Your account will be removed, are you sure?", "No", null, "Remove");
        
        if (confirmationRemoveUser == "Remove") 
        {
            IsBusy = true;
            var user = await userService.RetrieveUser(App.UserSessionData.Username);
            var removeRequest = await userService.RemoveUser(user);

            if (removeRequest == true) 
            {
                if (Preferences.ContainsKey(nameof(App.UserSessionData))) 
                {
                    Preferences.Remove(nameof(App.UserSessionData));
                }
                IsBusy = false;
                await Shell.Current.DisplayAlert("Successfull request", "Your user account has been removed.", "OK");
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
        }
    }
}

