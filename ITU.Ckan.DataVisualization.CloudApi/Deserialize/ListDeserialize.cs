using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.CloudApi.Deserialize
{
    public class ListDeserialize
    {
        public string help { get; set; }
        public bool success { get; set; }
        public List<String> result { get; set; }
    }
}
