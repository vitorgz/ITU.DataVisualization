using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl
{
    public class RootFactory : IRootFactory
    {

        private Root root;
        public IRootFactory Initialize()
        {
            root = new Root();
            return this;
        }

        public IRootFactory AddVisualization(string name)
        {
            if (root.Visualizations == null)
                root.Visualizations = new List<Visualization>();

            (root.Visualizations as List<Visualization>).Add(new Visualization() { Name = name });

            return this;
        }
       
        public Root Create()
        {
            return root;
        }

        
    }
}
