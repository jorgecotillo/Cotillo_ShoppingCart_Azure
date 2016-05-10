using Cotillo_ShoppingCart_Services.Domain.Model.User;
using Cotillo_ShoppingCart_Services.Integration.Implementation.EF;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Azure.App_Start
{
    /// <summary>
    /// 
    /// </summary>
    public class SomeIdentityUser : IdentityUser<int, SomeIdentityUserLogin, SomeIdentityUserRole, SomeIdentityUserClaim>, IUser<int>
    {
        public static int _id = 0;
        public static object _lockObject = new object();
        public int Id
        {
            get
            {
                lock(_lockObject)
                {
                    return _id++;
                }
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SomeIdentityUserLogin : IdentityUserLogin<int>
    {

    }
    /// <summary>
    /// 
    /// </summary>
    public class SomeIdentityRole : IdentityRole<int, SomeIdentityUserRole>
    {

    }
    /// <summary>
    /// 
    /// </summary>
    public class SomeIdentityUserRole : IdentityUserRole<int>
    {

    }
    /// <summary>
    /// 
    /// </summary>
    public class SomeIdentityUserClaim : IdentityUserClaim<int>
    {

    }
    /// <summary>
    /// 
    /// </summary>
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<SomeIdentityUser, int>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="store"></param>
        public ApplicationUserManager(IUserStore<SomeIdentityUser, int> store)
            : base(store)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<SomeIdentityUser, SomeIdentityRole, int, SomeIdentityUserLogin, SomeIdentityUserRole, SomeIdentityUserClaim>(context.Get<EFContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<SomeIdentityUser, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<SomeIdentityUser, int>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<SomeIdentityUser, int>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            //manager.EmailService = new EmailService();
            //manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<SomeIdentityUser, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    public class ApplicationSignInManager : SignInManager<SomeIdentityUser, int>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) :
            base(userManager, authenticationManager)
        { }

        public override async Task<ClaimsIdentity> CreateUserIdentityAsync(SomeIdentityUser user)
        {
            user.Claims.Add(new SomeIdentityUserClaim() { ClaimType = "Roles", ClaimValue = "a,b,c" });
            return await CreateUserIdentityAsync(user);
        }

        //public override Task<SomeIdentityUserClaim> CreateUserIdentityAsync(SomeIdentityUser user)
        //{
        //    var claims = user.Claims;
        //    claims.Add(new SomeIdentityUserClaim() { ClaimType = "Roles", ClaimValue = "a,b,c" });
        //    return claims;
        //}

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
