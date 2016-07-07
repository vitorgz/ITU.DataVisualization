using ITU.Ckan.DataVisualization.InternalDsl.ExtensionMethods;
using ITU.Ckan.DataVisualization.InternalDslApi.DTO;
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

        private static SourceDTO serieX;
        private static SourceDTO serie1;
        private static SourceDTO serie2;

        public static SourceDTO SerieX
        {
            get
            {
                if (serieX == null)
                    serieX = new SourceDTO();
                return serieX;
            }
        }

        public static SourceDTO Serie1
        {
            get
            {
                if (serie1 == null)
                    serie1 = new SourceDTO();
                return serie1;
            }
        }

        public static SourceDTO Serie2
        {
            get
            {
                if (serie2 == null)
                    serie2 = new SourceDTO();
                return serie2;
            }
        }

        public static void SelectFields()
        {
            //var filters = new VisualDTO();
            //var listDtos = new List<SourceDTO>();

            var visual = CurrentVisualization;
            visual.restartSeries(); //CHECK!

            
            if (SerieX.fields != null && SerieX.fields.FirstOrDefault() != null)
            {
                var source = visual.GetSourceById(x => x.name == SerieX.sourceName);
                var ds = source.GetPackageByName(x => x.name == SerieX.packageName).dataSets;
                var fields = ds.Where(x=>x.id == SerieX.dataSetId).FirstOrDefault().fields;
                var field = fields.Where(x => x.name == SerieX.fields.FirstOrDefault().name);

                field.FirstOrDefault().xAxys = true;

                if (visual.graph.name == "PieChart") return;
            }
            if (Serie1.fields != null && Serie1.fields.FirstOrDefault() != null)
            {
                var source = visual.GetSourceById(x => x.name == Serie1.sourceName);
                var ds = source.GetPackageByName(x => x.name == Serie1.packageName).dataSets;
                var fields = ds.Where(x => x.id == Serie1.dataSetId).FirstOrDefault().fields;
                var field = fields.Where(x => x.name == Serie1.fields.FirstOrDefault().name);

                field.FirstOrDefault().selected = true;

            }
            if (Serie2.fields != null && Serie2.fields.FirstOrDefault() != null)
            {
                var source = visual.GetSourceById(x => x.name == Serie2.sourceName);
                var ds = source.GetPackageByName(x => x.name == Serie2.packageName).dataSets;
                var fields = ds.Where(x => x.id == Serie2.dataSetId).FirstOrDefault().fields;
                var field = fields.Where(x => x.name == Serie2.fields.FirstOrDefault().name);

                field.FirstOrDefault().selected = true;
            }
            
        }
    }
}