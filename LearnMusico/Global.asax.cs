using LearnMusico.Common;
using LearnMusico.Init;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LearnMusico
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();    
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            App.Common = new WebCommon();
        }
    }
}
