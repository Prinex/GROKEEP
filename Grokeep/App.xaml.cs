namespace Grokeep;

public partial class App : Application
{
	private static User userSessionData = new User();

	public static User UserSessionData { get => userSessionData; set => userSessionData = value; }

	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}
