namespace Grokeep.ViewModels;

public partial class RememberedUserPageViewModel : BaseViewModel
{
    public RememberedUserPageViewModel()
    {
        OptedForStayingLoggedIn();
    }

    public async void OptedForStayingLoggedIn()
    {
        if (IsBusy == true)
            return;
        IsBusy = true;

        bool hasOpted = Preferences.Get("KeepUsrLoggedIn", false);

        if(hasOpted == true)
        {
            // deserializing the saved user credentials
            string serializedUserCredentials = Preferences.Get(nameof(App.UserSessionData), "");
            User deserializedUserCredentials = new User();

            if (!string.IsNullOrWhiteSpace(serializedUserCredentials)) 
            {
                deserializedUserCredentials = JsonConvert.DeserializeObject<User>(serializedUserCredentials);
                App.UserSessionData = deserializedUserCredentials;
            }
            IsBusy = false;
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }
        else
        {

            IsBusy = false;
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}

