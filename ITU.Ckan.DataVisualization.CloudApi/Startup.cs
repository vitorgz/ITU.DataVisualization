using ITU.Ckan.DataVisualization.CloudApi;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

//[assembly: OwinStartup(typeof(Startup))]
namespace ITU.Ckan.DataVisualization.CloudApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            //config.DependencyResolver = new NinjectDependencyResolver(CreateKernel());

            config.MapHttpAttributeRoutes();

            //string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            //GlobalHost.DependencyResolver.UseServiceBus(connectionString, "Chat");

            //app.MapSignalR();

            app.UseWebApi(config);
        }
    }
}
