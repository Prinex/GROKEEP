namespace Grokeep.ViewModels;
public partial class ForgotUserPasswordPageViewModel : BaseViewModel
{
    private readonly IUserService userService;
    [ObservableProperty]
    private User userCredentials = new User();

    public ForgotUserPasswordPageViewModel(IUserService userService)
    {
        this.userService = userService;
    }

    [RelayCommand]
    public async void Change()
    {
        if (IsBusy == true)
            return;

        IsBusy = true;

        var user = new User();
        bool changePwResponse = false;

        if (string.IsNullOrEmpty(UserCredentials.Username) || string.IsNullOrEmpty(UserCredentials.Password))
        {
            IsBusy = false;
            await Shell.Current.DisplayAlert("Warning", "Provide both username and the new password associated with your account.", "OK");
        }
        else if (!string.IsNullOrEmpty(UserCredentials.Username) && !string.IsNullOrEmpty(UserCredentials.Password))
        {
            user = await userService.RetrieveUser(UserCredentials.Username);
            if (user.DoesExist == false) 
            {
                IsBusy = false;
                await Shell.Current.DisplayAlert($"Error", $"{UserCredentials.Username} username does not exist.", "OK");
            }
            else
            {
                // update the pw with the new one as salted and hashed 
                var encoder = new ScryptEncoder();
                user.Password = encoder.Encode(UserCredentials.Password);
                changePwResponse = await userService.UpdateUser(user);

                if (changePwResponse == true)
                {
                    IsBusy = false;
                    await Shell.Current.DisplayAlert("Successfull update", "The password has been updated.", "OK");
                }
                else
                {
                    IsBusy = false;
                    await Shell.Current.DisplayAlert("Update error", "The password was not updated due to some technical issues.", "OK");
                }
            }
        }
    }
}

