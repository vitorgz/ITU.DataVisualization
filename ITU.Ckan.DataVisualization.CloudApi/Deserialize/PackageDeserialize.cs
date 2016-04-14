using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.CloudApi.Deserialize
{
    public class PackageDeserialize
    {
        public string help { get; set; }
        public bool success { get; set; }
        public Results result { get; set; }
    }

    public class Results {
        public string id { get; set; }
        public string author { get; set; }
        public string state { get; set;}
        public List<DataSet> resources { get; set; }
        public List<Tag> tags { get; set; }
        public List<Group> groups { get; set; }
        public Organization orgazation { get; set; }
    }
}
