using Grokeep.Services;
using Grokeep.ViewModels;
using Microsoft.Extensions.Logging;

namespace Grokeep.Views;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("RobotoBold", "RobotoB");
                fonts.AddFont("RobotoLight", "RobotoL");
                fonts.AddFont("RobotoRegular", "RobotoR");
            });

        // dependency injection services

        // note: using mostly trasient for the pages where the main features are due to passing
        // states, objects, and parameters and changing them between when navigating
        
        // registering services 
        builder.Services.AddSingleton< IUserService, UserService>();

        // registering views
        builder.Services.AddSingleton<TermsOfUsePage>();
        builder.Services.AddSingleton<PrivacyPage>();
        builder.Services.AddSingleton<AppInfoPage>();
        builder.Services.AddSingleton<RememberedUserPage>();
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<RegisterPage>();
        builder.Services.AddTransient<RegisterPrivacyPolicyPage>();
        builder.Services.AddTransient<RegisterTermsAndConditionsPage>();
        builder.Services.AddTransient<ForgotUserPasswordPage>();
        builder.Services.AddSingleton<MainPage>();

        // registering viewmodels
        builder.Services.AddSingleton<BaseViewModel>();
        builder.Services.AddTransient<RememberedUserPageViewModel>();
        builder.Services.AddSingleton<LoginPageViewModel>();
        builder.Services.AddSingleton<RegisterPageViewModel>();
        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddTransient<ForgotUserPasswordPageViewModel>();

        return builder.Build();
    }
}
