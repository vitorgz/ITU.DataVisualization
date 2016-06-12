using System.ComponentModel.DataAnnotations;

namespace ITU.Ckan.DataVisualization.EFDataBase
{
    public class VisualizationDB
    {
        [Key]
        public int Id { get; set; }

        public string name { get; set; }

        public string visualizationAsJson { get; set; }

    }
}