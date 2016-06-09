using ITU.Ckan.DataVisualization.InternalDsl.IFactories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl
{
    public class RootFactory : IRootFactory
    {
        private static Root root;

        private static IRootFactory rootf;

        public static IRootFactory Initialize
        {
            get
            {
                if (rootf == null)
                    rootf = new RootFactory();
                if (root == null)
                    root = new Root();
                return rootf as IRootFactory;
            }
        }

        public IRootFactory AddIn(Action<IRootFactory> action)
        {
            var expression = RootFactory.Initialize;
            action.Invoke(expression);

            return this;
        }

        public IRootFactory AddVisualization(string name)
        {
            if (root.visualizations == null)
                root.visualizations = new List<Visualization>();

            (root.visualizations as List<Visualization>).Add(new Visualization() { name = name });

            return this;
        }
       
        public Root Create()
        {
            return root;
        }

        
    }
}
