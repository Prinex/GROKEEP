using Grokeep.Views;

namespace Grokeep;

public partial class AppShell : Shell
{
	public AppShell()
	{
        InitializeComponent();
	
		// registering routes to pages that are outside of appshell, or that are not displayed in the flyout like
		
		Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(ForgotUserPasswordPage), typeof(ForgotUserPasswordPage));
    }
}
