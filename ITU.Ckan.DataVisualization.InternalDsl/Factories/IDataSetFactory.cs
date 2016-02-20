using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl.Factories
{
    public interface IDataSetFactory
    {
        IDataSetFactory Initialize();
        IDataSetFactory AddGroup(List<Group> groups);
        IDataSetFactory AddOrganization(List<Organization> organizations);
        IDataSetFactory AddTag(List<Tag> tags);
        IDataSetFactory AddField(List<Field> field);
        IDataSetFactory AddRecord(List<Record> record);
        DataSet Create();
    }
}
