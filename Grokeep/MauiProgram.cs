using Grokeep.Services;
using Microsoft.Extensions.Logging;

namespace Grokeep.Views.Initial;

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
        
        // registering services singletons
        builder.Services.AddSingleton< IUserService, UserService>();


        // registering views

        // singletons
        builder.Services.AddSingleton<TermsOfUsePage>();
        builder.Services.AddSingleton<PrivacyPage>();
        builder.Services.AddSingleton<PrivacyPage>();
        builder.Services.AddSingleton<AppInfoPage>();
        // users login/registers, etc one time no need for transient
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<RegisterPage>();
        builder.Services.AddSingleton<RememberedUserPage>();
        // transients
        builder.Services.AddTransient<MainPage>();


        // registering viewmodels

        // singletons

        // transients

        return builder.Build();
    }
}
