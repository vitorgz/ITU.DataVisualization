using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITU.Ckan.DataVisualization.Web
{
    public class VisualizationInstance
    {
        private static Visualization current;

        public static Visualization Current
        {
            get
            {
                if (current == null)
                {
                    current = new Visualization() { id = 1};
                }
                return current;
            }
        }
    }
}