using ITU.Ckan.DataVisualization.InternalDsl.IFactories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl.Factories
{
   public class DataSetFactory : IDataSetFactory
    {
        private static DataSet dataSet;
        private static IDataSetFactory dataSetf;
        public static IDataSetFactory Initialize
        {
            get
            {
                if (dataSetf == null)
                    dataSetf = new DataSetFactory();
                if (dataSet == null)
                    dataSet = new DataSet();
                return dataSetf as IDataSetFactory;
            }
        }

        public IDataSetFactory AddIn(Action<IDataSetFactory> action)
        {
            var expression = DataSetFactory.Initialize;
            action.Invoke(expression);

            return this;
        }

        public IDataSetFactory AddField(List<Field> field)
        {
            dataSet.fields = new List<Field>(field);
            return this;
        }

        public IDataSetFactory AddGroup(List<Group> groups)
        {
            //TODO -> it is a relationship, not new entities, go the (group, organi., tag, records) table 
            //var group = source.GetGroups.Where(x=>x.dataSet = id);
            return this;
        }

        public IDataSetFactory AddOrganization(Organization organization)
        {
            //Todo
            return this;
        }

        public IDataSetFactory AddTag(List<Tag> tags)
        {
            //todo
            return this;
        }
        public IDataSetFactory GetRecords()
        {
            //todo
            return this;
        }

        public DataSet Create()
        {
            return dataSet;
        }
    }
}
