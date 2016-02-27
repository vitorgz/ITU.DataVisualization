using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.CloudApi.DTO
{
    public class PackageListDTO
    {
        public string help { get; set; }
        public bool success { get; set; }
        public List<String> result { get; set; }
    }
}
