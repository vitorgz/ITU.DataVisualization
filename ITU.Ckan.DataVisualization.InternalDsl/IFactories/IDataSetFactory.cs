using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl.IFactories
{
    public interface IDataSetFactory
    {
        IDataSetFactory AddIn(Action<IDataSetFactory> action);
        //IDataSetFactory Initialize();
        IDataSetFactory AddGroup(List<Group> groups);
        IDataSetFactory AddOrganization(Organization organization);
        IDataSetFactory AddTag(List<Tag> tags);
        IDataSetFactory AddField(List<Field> field);
        IDataSetFactory GetRecords();
        DataSet Create();
    }
}
