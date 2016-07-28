using ITU.Ckan.DataVisualization.InternalDsl.IFactories;
using ITU.Ckan.DataVisualization.InternalDslApi;
using ITU.Ckan.DataVisualization.InternalDslApi.DTO;
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
        public IDataSetFactory Initialize()
        {
            dataSetf = new DataSetFactory();
            dataSet = new DataSet();
            return dataSetf as IDataSetFactory;
        }

        public IDataSetFactory AddIn(Action<IDataSetFactory> action)
        {
            var expression = dataSetf ?? Initialize();
            action.Invoke(dataSetf);

            return this;
        }

        public IDataSetFactory AddField(List<Field> field)
        {
            dataSet.fields = new List<Field>(field);
            return this;
        }

        public IDataSetFactory AddGroup(List<Group> groups)
        {
            //it is a relationship, ideally it would be better: 
            //var group = source.GetGroups.Where(x=>x.dataSet = id);
            dataSet.groups = new List<Group>(groups);
            return this;
        }

        public IDataSetFactory AddOrganization(Organization organization)
        {
            dataSet.organization = organization;
            return this;
        }

        public IDataSetFactory AddTag(List<Tag> tags)
        {
            dataSet.tags = new List<Tag>(tags);
            return this;
        }

        public IDataSetFactory GetMetaData(string sourceName, string dataSetId)
        {
            var dto = new SourceDTO();
            dto.sourceName = sourceName;
            dto.dataSetId = dataSetId;

            var task = Task.Run(async () =>
            {
                var fields = await InternalClient.GetMetaData(dto);
                dataSet.fields = fields;
            });
            task.Wait();
            return this;
        }

        public DataSet Create()
        {
            return dataSet;
        }
    }
}
