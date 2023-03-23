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
        
        // registering services 
        builder.Services.AddSingleton< IUserService, UserService>();
        builder.Services.AddSingleton<IGroceryInventoryService, GroceryInventoryService>();
        builder.Services.AddSingleton<IGroceryHistoryService, GroceryHistoryService>();
        builder.Services.AddSingleton<IProductService, ProductService>();

        // registering views
        builder.Services.AddSingleton<TermsOfUsePage>();
        builder.Services.AddSingleton<PrivacyPage>();
        builder.Services.AddSingleton<PrivacyPage>();
        builder.Services.AddSingleton<AppInfoPage>();
        builder.Services.AddSingleton<RememberedUserPage>();
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<ForgotUserPasswordPage>();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<GroceryInventoriesPage>();

        // registering viewmodels
        builder.Services.AddTransient<RememberedUserPageViewModel>();
        builder.Services.AddSingleton<LoginPageViewModel>();
        builder.Services.AddTransient<RegisterPageViewModel>();
        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddTransient<ForgotUserPasswordPageViewModel>();
        builder.Services.AddTransient<ProfilePageViewModel>();
        builder.Services.AddTransient<GroceryInventoriesPageViewModel>();
        return builder.Build();
    }
}
