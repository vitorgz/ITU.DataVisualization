using ITU.Ckan.DataVisualization.EFDataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDslApi
{
    public static class DBClient
    {
        static VisualizationDBContext context = new VisualizationDBContext();

        public static void CreateVisualization(string name)
        {
            context.Visualizations.Add(new Visualization() { name = name } );
            context.SaveChanges();    
        }

        public static void SaveVisualization(Visualization vs)
        {
            context.Visualizations.Add(vs);
            context.SaveChanges();
        }
    }
}
