using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl
{
    public interface IRootFactory
    {
        IRootFactory Initialize();
        IRootFactory AddSource(string firstName);
        Root Create();
    }
}
