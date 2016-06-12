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
                    current.visualizations = new List<Visualization>();
                    current.graphs = new List<Graph>() {
                        new Graph() { name = "PieChart"},
                        new Graph() { name = "LineChart"},
                        new Graph() { name = "ColumnChart"},
                        new Graph() { name = "BarChart"}
                    };
                }
                return current;
            }
        }

        public static Visualization CurrentVisualization { get; set; }
    }
}