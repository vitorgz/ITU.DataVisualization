using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDslApi.DTO
{
    public class VisualDTO
    {
        public string name { get; set; }

        public List<SourceDTO> sources { get; set; }

        public int limit { get; set; }

        public int offset { get; set; }
    }

}
