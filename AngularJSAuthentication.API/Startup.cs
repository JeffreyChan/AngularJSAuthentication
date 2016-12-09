using AngularJSAuthentication.API.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using AngularJSAuthentication.API.Controllers;
using AngularJSAuthentication.API.Filter;
using AngularJSAuthentication.API.Identity;
using AngularJSAuthentication.API.Logger;
using AngularJSAuthentication.API.Middleware;
using AngularJSAuthentication.API.Models;
using AngularJSAuthentication.API.Repository;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;

[assembly: OwinStartup(typeof(AngularJSAuthentication.API.Startup))]

namespace AngularJSAuthentication.API
{
    public class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        public static GoogleOAuth2AuthenticationOptions googleAuthOptions { get; private set; }
        public static FacebookAuthenticationOptions facebookAuthOptions { get; private set; }

        public void Configuration(IAppBuilder app)
        {

            var config = new HttpConfiguration();



            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            var builder = new ContainerBuilder();

            builder.RegisterType<AuthContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationUserStore>().As<IUserStore<ApplicationUser>>().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();

            //builder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.Register<IDataProtectionProvider>(c => app.GetDataProtectionProvider()).InstancePerRequest();

            // Register Web API controller in executing assembly.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL - Register the filter provider if you have custom filters that need DI.
            // Also hook the filters up to controllers.
            builder.RegisterWebApiFilterProvider(config);
            builder.RegisterType<CustomActionFilter>()
                .AsWebApiActionFilterFor<TestController>()
                .InstancePerRequest();



            // Register a logger service to be used by the controller and middleware.
            builder.RegisterType<MyLogger>().As<IMyLogger>().InstancePerRequest();
            builder.RegisterType<AuthRepository>().As<IAuthRepository>().InstancePerRequest();


            // Autofac will add middleware to IAppBuilder in the order registered.
            // The middleware will execute in the order added to IAppBuilder.
            builder.RegisterType<FirstMiddleware>().InstancePerRequest();
            builder.RegisterType<SecondMiddleware>().InstancePerRequest();



            // Create and assign a dependency resolver for Web API to use.
            var container = builder.Build();



            var resolver = new AutofacWebApiDependencyResolver(container);

            config.DependencyResolver = resolver;

            // The Autofac middleware should be the first middleware added to the IAppBuilder.
            // If you "UseAutofacMiddleware" then all of the middleware in the container
            // will be injected into the pipeline right after the Autofac lifetime scope
            // is created/injected.
            //
            // Alternatively, you can control when container-based
            // middleware is used by using "UseAutofacLifetimeScopeInjector" along with
            // "UseMiddlewareFromContainer". As long as the lifetime scope injector
            // comes first, everything is good.
            app.UseAutofacMiddleware(container);

            // Again, the alternative to "UseAutofacMiddleware" is something like this:
            // app.UseAutofacLifetimeScopeInjector(container);
            // app.UseMiddlewareFromContainer<FirstMiddleware>();
            // app.UseMiddlewareFromContainer<SecondMiddleware>();

            // Make sure the Autofac lifetime scope is passed to Web API.
            app.UseAutofacWebApi(config);

            app.UseWebApi(config);

            ConfigureOAuth(app, container);
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AuthContext, Migrations.Configuration>());


        }

        public void ConfigureOAuth(IAppBuilder app, IContainer container)
        {
            //use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            var oauthServerOptions = new OAuthAuthorizationServerOptions()
            {

                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new SimpleAuthorizationServerProvider(),
                RefreshTokenProvider = new SimpleRefreshTokenProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(oauthServerOptions);
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);

            //Configure Google External Login
            googleAuthOptions = new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "xxxxxx",
                ClientSecret = "xxxxxx",
                Provider = new GoogleAuthProvider()
            };
            app.UseGoogleAuthentication(googleAuthOptions);

            //Configure Facebook External Login
            facebookAuthOptions = new FacebookAuthenticationOptions()
            {
                AppId = "xxxxxx",
                AppSecret = "xxxxxx",
                Provider = new FacebookAuthProvider()
            };
            app.UseFacebookAuthentication(facebookAuthOptions);

        }
    }

}