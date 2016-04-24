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
        static Source source;
        //public static Source Initialize
        //{
        //    get{
        //        if(source == null) 
        //            source = new Source();
        //        return source;
        //    }
        //}

        static SourceFactory sourcef;
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

        public Source Create()
        {
            return source;
        }

        public ISourceFactory GetPackages(string src)
        {
            //CHECK!
            //todo change it to <List<DataSet>> make more sense
            Task.Factory.StartNew(() =>
            {
                var tas = InternalClient.GetPackages<List<Package>>(src);
                source.packages = tas.Result;
            }).Wait();

            return this;
            //var sources = await InternalClient.GetPackages<List<Package>>(source);
            //this.source.packages = sources;

            //return this;
        }

        public async Task<ISourceFactory> GetGroups(string scr)
        {
            var groups = await InternalClient.GetGroups<List<Group>>(scr);
            source.groups = groups;

            return this;
        }

        public async Task<ISourceFactory> GetOrganizations(string scr)
        {
            var orga = await InternalClient.GetOrganizations<List<Organization>>(scr);
            source.organizations = orga;

            return this;
        }

        public async Task<ISourceFactory> GetTag(string scr)
        {
            var tags = await InternalClient.GetTags<List<Tag>>(scr);
            source.tags = tags;

            return this;
        }

        public ISourceFactory AddIn(Action<ISourceFactory> action)
        {
            var expression = SourceFactory.Initialize;
            action.Invoke(expression);

            return this;
        }

        public Source getSource
        {
            get { return source; }
        }
    }
    
}
