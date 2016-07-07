using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDslApi.DTO
{
    public class SourceDTO
    {
        public string dataSetId { get; set; }
        public string sourceName { get; set; }
        public string packageName { get; set; }
        public List<Field> fields { get; set; }

        public int limit { get; set; }
        public int offset { get; set; }
        public string command { get; set; }
    }
}
