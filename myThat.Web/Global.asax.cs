using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using ServiceStack.ServiceInterface;
using ServiceStack.WebHost.Endpoints;
using myThat.ServiceInterface;

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
            public myThatAppHost() : base("Web Services", typeof(SpeakerService).Assembly)
            { }

            public override void Configure(Funq.Container container)
            {

            }
        }
    }
}