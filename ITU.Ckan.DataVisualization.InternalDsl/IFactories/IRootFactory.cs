using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl.IFactories
{
    public interface IRootFactory
    {
        //IRootFactory Initialize();
        IRootFactory AddIn(Action<IRootFactory> action);
        IRootFactory AddVisualization(string firstName);
        Root Create();
    }
}
