﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using ServiceStack.Authentication.OpenId;
using ServiceStack.CacheAccess;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.MiniProfiler;
using ServiceStack.MiniProfiler.Data;
using ServiceStack.Mvc;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.SqlServer;
using ServiceStack.Redis;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.ServiceInterface.Validation;
using ServiceStack.WebHost.Endpoints;
using SharpStack.Data.Database;
using myThat.ServiceInterface;
using myThat.ServiceInterface.Mappings;
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
                }) {HtmlRedirect = "/Camper/SignIn"});

                //Provide service for new users to register so they can login with supplied credentials.
                Plugins.Add(new RegistrationFeature());

                NHibernateConfigurator.Initialize<CamperMapping>();
                //Store User Data into the referenced SqlServer database
                container.Register<IUserAuthRepository>(c =>
                    new NHibernateUserAuthRepository()); //Can Use OrmLite DB Connection to persist the UserAuth and AuthProvider info

                //override the default registration validation with your own custom implementation
                container.RegisterAs<MyRegistrationValidator, IValidator<Registration>>();

                var redisCon = ConfigurationManager.AppSettings["redisUrl"].ToString();
                container.Register<IRedisClientsManager>(new PooledRedisClientManager(20, 60, redisCon));
                container.Register<ICacheClient>(c => (ICacheClient)c.Resolve<IRedisClientsManager>().GetCacheClient());

                ControllerBuilder.Current.SetControllerFactory(new FunqControllerFactory(container));
            }
        }
    }
}