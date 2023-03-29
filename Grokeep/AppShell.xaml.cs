namespace Grokeep;

public partial class AppShell : Shell
{
	public AppShell()
	{
        InitializeComponent();
		this.BindingContext = new BaseViewModel();
	
		// registering routes to pages that are outside of appshell, or that are not displayed in the flyout like
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(ForgotUserPasswordPage), typeof(ForgotUserPasswordPage));
		Routing.RegisterRoute(nameof(RegisterPrivacyPolicyPage), typeof(RegisterPrivacyPolicyPage));
		Routing.RegisterRoute(nameof(RegisterTermsAndConditionsPage), typeof(RegisterTermsAndConditionsPage));
    }
}
