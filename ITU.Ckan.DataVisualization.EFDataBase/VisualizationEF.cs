using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.EFDataBase
{
    public class VisualizationEF
    {
        [Key]
        public int Id { get; set; }
        public Visualization Visualization { get; set; }

    }
}
