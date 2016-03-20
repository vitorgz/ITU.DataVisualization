using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.CloudApi.DTO
{
    public class DataSetDTO
    {
        public string help { get; set; }
        public bool success { get; set; }
        public DataSet result { get; set; }
        //public List<DataSet> result { get; set; }
    }

    public class DataSetIn
    {
        public string resource_id { get; set; }
        public List<Field> fields { get; set; }
        //public List<object> records { get; set; }
    }
}
