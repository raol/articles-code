using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;

namespace SampleWorkerRole
{
    using System.Xml;
    using log4net;
    using log4net.Config;

    public class WorkerRole : RoleEntryPoint
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(WorkerRole));

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Log.Info("SampleWorkerRole is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            if (RoleEnvironment.IsAvailable)
            {
                try
                {
                    ConfigureLogging();
                    RoleEnvironment.Changed += RoleEnvironmentOnChanged;
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Error configuring log4net. Details {0}", ex);
                }
                
            }

            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Log.Info("SampleWorkerRole has been started");

            return result;
        }

        private void RoleEnvironmentOnChanged(object sender, RoleEnvironmentChangedEventArgs roleEnvironmentChangedEventArgs)
        {
            var log4netChange = roleEnvironmentChangedEventArgs.Changes
                .OfType<RoleEnvironmentConfigurationSettingChange>()
                .FirstOrDefault(c => string.Equals(c.ConfigurationSettingName, "log4net", StringComparison.OrdinalIgnoreCase));
            if (log4netChange == null)
            {
                return;
            }

            ConfigureLogging();
        }

        public override void OnStop()
        {
            Log.Info("SampleWorkerRole is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Log.Info("SampleWorkerRole has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Log.Info("Working");
                await Task.Delay(1000);
            }
        }

        private void ConfigureLogging()
        {
            var log4netConfig = CloudConfigurationManager.GetSetting("log4net");
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(log4netConfig);

            XmlConfigurator.Configure(xmlDocument.DocumentElement);
        }
    }
}
