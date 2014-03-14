using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using ServiceStack.Authentication.OpenId;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.MiniProfiler;
using ServiceStack.MiniProfiler.Data;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.SqlServer;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.ServiceInterface.Validation;
using ServiceStack.WebHost.Endpoints;
using myThat.ServiceInterface;
using myThat.ServiceModel.Validators;

namespace myThat
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
            new myThatAppHost().Init();
        }

        public class myThatAppHost : AppHostBase
        {
            public myThatAppHost() : base("Web Services", typeof(CamperService).Assembly)
            { }

            public override void Configure(Funq.Container container)
            {
                //Set JSON web services to return idiomatic JSON camelCase properties
                ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;
                var appSettings = new AppSettings();
                //Registers authorization service and endpoints /auth and /auth{provider}

                Plugins.Add(new AuthFeature(() => new AuthUserSession(), 
                    new IAuthProvider[] {
                    new CredentialsAuthProvider(),              //HTML Form post of UserName/Password credentials
                    new TwitterAuthProvider(appSettings),       //Sign-in with Twitter
                    //new FacebookAuthProvider(appSettings),      //Sign-in with Facebook
                    new GoogleOpenIdOAuthProvider(appSettings), //Sign-in with Google OpenId
                }));

                //Provide service for new users to register so they can login with supplied credentials.
                Plugins.Add(new RegistrationFeature());

                container.Register<IDbConnectionFactory>(new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["myThat"].ToString(), //ConnectionString in Web.Config
                   SqlServerOrmLiteDialectProvider.Instance)
                   {
                       ConnectionFilter = x => new ProfiledDbConnection(x, Profiler.Current)
                   });

                //Store User Data into the referenced SqlServer database
                container.Register<IUserAuthRepository>(c =>
                    new OrmLiteAuthRepository(c.Resolve<IDbConnectionFactory>())); //Use OrmLite DB Connection to persist the UserAuth and AuthProvider info

                var authRepo = (OrmLiteAuthRepository)container.Resolve<IUserAuthRepository>(); //If using and RDBMS to persist UserAuth, we must create required tables
                if (appSettings.Get("RecreateAuthTables", false))
                    authRepo.DropAndReCreateTables(); //Drop and re-create all Auth and registration tables
                else
                    authRepo.CreateMissingTables();   //Create only the missing tables

                //override the default registration validation with your own custom implementation
                container.RegisterAs<MyRegistrationValidator, IValidator<Registration>>();
            }
        }
    }
}