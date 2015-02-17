using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SampleWebRole
{
    using System.Diagnostics;
    using System.Xml;
    using log4net;
    using log4net.Config;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.ServiceRuntime;

    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MvcApplication));

        protected void Application_Start()
        {
            if (RoleEnvironment.IsAvailable)
            {
                try
                {
                    var log4netConfig = CloudConfigurationManager.GetSetting("log4net");
                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(log4netConfig);

                    XmlConfigurator.Configure(xmlDocument.DocumentElement);
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Error configuring log4net. Details {0}", ex);
                }
            }

            Log.Info("Starting web application");
            
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
