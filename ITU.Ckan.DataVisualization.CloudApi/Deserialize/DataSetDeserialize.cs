using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.CloudApi.Deserialize
{
    public class DataSetDeserialize
    {
        public string help { get; set; }
        public bool success { get; set; }
        public DataSet result { get; set; }
    }

    public class DataSetIn
    {
        public string resource_id { get; set; }
        public string id { get; set; }
        public List<Field> fields { get; set; }
        public int total { get; set;}
        public int limit { get; set; }
        //Records is dynamic
        //public List<object> records { get; set; }
    }
}
