using Cotillo_ShoppingCart_Services.Migrations;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Authentication;
using Microsoft.Azure.Mobile.Server.Config;
using Owin;
using System.Configuration;
using System.Data.Entity;
using System.Web.Http;

namespace Cotillo_ShoppingCart_Azure
{
    public partial class Startup
    {
        /// <summary>
        /// OWIN Startup class: Configures the mobile application.
        /// </summary>
        /// <param name="app">The application.</param>
        public static void ConfigureMobileApp(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            new MobileAppConfiguration()
                .AddMobileAppHomeController()
                .AddPushNotifications()
                .AddTables()
                .ApplyTo(config);
                //.UseDefaultConfiguration()
                //.ApplyTo(config);

            MobileAppSettingsDictionary settings = config.GetMobileAppSettingsProvider().GetMobileAppSettings();

            // Use Entity Framework Code First to create database tables based on your DbContext
            Database.SetInitializer(new MobileServiceInitializer());
            //var migrator = new DbMigrator(new Cotillo_ShoppingCart_Services.Migrations.Configuration());
            //migrator.Update();

            if (string.IsNullOrEmpty(settings.HostName))
            {
                app.UseAppServiceAuthentication(new AppServiceAuthenticationOptions
                {
                    // This middleware is intended to be used locally for debugging. By default, HostName will
                    // only have a value when running in an App Service application.
                    SigningKey = ConfigurationManager.AppSettings["SigningKey"],
                    ValidAudiences = new[] { ConfigurationManager.AppSettings["ValidAudience"] },
                    ValidIssuers = new[] { ConfigurationManager.AppSettings["ValidIssuer"] },
                    TokenHandler = config.GetAppServiceTokenHandler()
                });
            }

            app.UseWebApi(config);
        }
    }
}
