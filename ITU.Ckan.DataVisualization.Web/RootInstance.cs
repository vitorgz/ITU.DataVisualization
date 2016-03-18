using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITU.Ckan.DataVisualization.Web
{
    public class RootInstance
    {
        private static Root current;

        public static Root Current
        {
            get
            {
                if (current == null)
                {
                    current = new Root();
                }
                return current;
            }
        }
    }
}