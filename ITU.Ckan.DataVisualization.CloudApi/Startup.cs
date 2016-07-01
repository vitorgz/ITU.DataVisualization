using ITU.Ckan.DataVisualization.CloudApi;
using ITU.Ckan.DataVisualization.EFDataBase;
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
        public static VisualizationDBContext context;

        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            //config.DependencyResolver = new NinjectDependencyResolver(CreateKernel());

            config.MapHttpAttributeRoutes();


            context = new VisualizationDBContext();

            app.UseWebApi(config);
        }
    }
}
