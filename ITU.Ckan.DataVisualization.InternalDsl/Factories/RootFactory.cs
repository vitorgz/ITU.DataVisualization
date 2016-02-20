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

        public IRootFactory AddSource(string name)
        {
            if (root.Sources == null)
                root.Sources = new List<Source>();

            (root.Sources as List<Source>).Add(new Source() { Name = name });

            return this;
        }
       
        public Root Create()
        {
            return root;
        }

        
    }
}
