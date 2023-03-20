namespace Grokeep.ViewModels;
public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    public bool isBusy;

    [ObservableProperty]
    public string title;

    // functionalitiy and properties for showing-hiding the password
    [ObservableProperty]
    public bool passVisibility = true;

    [ObservableProperty]
    public string passVisibImg = "hide.png";

    [RelayCommand]
    public void HidePass()
    {
        if (PassVisibImg == "hide.png")
        {
            PassVisibImg = "show.png";
            PassVisibility = false;
        }
        else if (PassVisibImg == "show.png")
        {
            PassVisibImg = "hide.png";
            PassVisibility = true;
        }
    }

    public async void LogOut()
    {
        if (Preferences.ContainsKey("KeepUsrLoggedIn"))
        {
            Preferences.Remove("KeepUsrLoggedIn");
        }
        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }
}

