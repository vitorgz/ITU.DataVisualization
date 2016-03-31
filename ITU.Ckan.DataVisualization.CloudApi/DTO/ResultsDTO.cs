using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.CloudApi.DTO
{
    public class ResultsDTO
    {
        public string help { get; set; }
        public bool success { get; set; }
        public RecordsDTO result { get; set; }
    }
    public class RecordsDTO
    {
        public List<RecordDTO> records { get; set; }
    }

    public class RecordDTO
    {
    }

    
}
