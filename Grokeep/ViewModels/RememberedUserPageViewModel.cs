using Newtonsoft.Json;

namespace Grokeep.ViewModels;

public partial class RememberedUserPageViewModel : BaseViewModel
{
    public RememberedUserPageViewModel()
    {
        OptedForStayingLoggedIn();
    }

    public async void OptedForStayingLoggedIn()
    {
        bool hasOpted = Preferences.Get("KeepUsrLoggedIn", false);

        if(hasOpted == true)
        {
            // deserializing the saved user credentials
            string serializedUserCredentials = Preferences.Get(nameof(App.UserSessionData), "");
            User deserializedUserCredentials = new User();

            if (serializedUserCredentials != "") 
            {
                deserializedUserCredentials = JsonConvert.DeserializeObject<User>(serializedUserCredentials);
                App.UserSessionData = deserializedUserCredentials;
            }
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }
        else
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}

