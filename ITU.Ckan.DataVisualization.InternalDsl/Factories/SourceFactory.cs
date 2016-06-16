using ITU.Ckan.DataVisualization.InternalDsl.IFactories;
using ITU.Ckan.DataVisualization.InternalDslApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl.Factories
{
    public class SourceFactory : ISourceFactory
    {
        private static Source source;

        private static SourceFactory sourcef;

        public static ISourceFactory Initialize
        {
            get
            {
                if (sourcef == null)
                    sourcef = new SourceFactory();
                if (source == null)
                    source = new Source();
                return sourcef as ISourceFactory;
            }
        }
        
        public ISourceFactory AddIn(Action<ISourceFactory> action)
        {
            var expression = SourceFactory.Initialize;
            action.Invoke(expression);
            
            return this;
        }

        public Source Create()
        {
            return source;
        }

        #region adds

        public ISourceFactory AddGroup(List<Group> groups)
        {
            source.groups = new List<Group>(groups);
            return this;
        }

        public ISourceFactory AddOrganization(List<Organization> organizations)
        {
            source.organizations = new List<Organization>(organizations);
            return this;
        }

        public ISourceFactory AddPackage(List<Package> packages)
        {
            source.packages = new List<Package>(packages);
            return this;
        }

        public ISourceFactory AddTag(List<Tag> tags)
        {
            source.tags = new List<Tag>(tags);
            return this;
        }

        #endregion
        
        #region sync methods

        public ISourceFactory GetPackages(string src)
        {
            var task = Task.Run(async () =>
            {
                var tas = await InternalClient.GetPackages<List<Package>>(src);
                source.packages = tas;
            });
            task.Wait();

            return this;
        }
        
        public ISourceFactory GetGroups(string src)
        {
            var task = Task.Run(async () =>
            {
                var tas = await InternalClient.GetGroups<List<Group>>(src);
                source.groups = tas;
            });
            task.Wait();

            return this;
        }

        public ISourceFactory GetOrganizations(string src)
        {
            var task = Task.Run(async () =>
            {
                var tas = await InternalClient.GetOrganizations<List<Organization>>(src);
                source.organizations = tas;
            });
            task.Wait();

            return this;
        }

        public ISourceFactory GetTag(string src)
        {
            var task = Task.Run(async () =>
            {
                var tas = await InternalClient.GetTags<List<Tag>>(src);
                source.tags = tas;
            });
            task.Wait();

            return this;
        }

        public Source getSource
        {
            get { return source; }
        }

        #endregion
    }

}
