namespace Grokeep.ViewModels;
    
public partial class MainPageViewModel : BaseViewModel
{
    [ObservableProperty]
    private string accountUsername;

    public MainPageViewModel()
    {
        if (App.UserSessionData != null) 
        {
            accountUsername = App.UserSessionData.Username;
        }
    }

    public void DisplayAccountUsername()
    {
        if (App.UserSessionData != null)
        {
            AccountUsername = App.UserSessionData.Username;
        }
    }
}

