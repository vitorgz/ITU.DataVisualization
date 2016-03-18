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
        Source source;
        public ISourceFactory Initialize()
        {
            source = new Source();
            return this;
        }

        public ISourceFactory AddGroup(List<Group> groups)
        {
            throw new NotImplementedException();
        }

        public ISourceFactory AddOrganization(List<Organization> organizations)
        {
            throw new NotImplementedException();
        }

        public ISourceFactory AddPackage(List<Package> packages)
        {
            throw new NotImplementedException();
        }

        public ISourceFactory AddTag(List<Tag> tags)
        {
            throw new NotImplementedException();
        }

        public Source Create()
        {
            throw new NotImplementedException();
        }

        public async Task<ISourceFactory> GetPackages(Source id)
        {

            var sources = await InternalClient.Get<List<Package>>(id.name);

            this.source.packages = sources;

            return this;
        }

        public ISourceFactory GetSources()
        {
            throw new NotImplementedException();
        }
    }
}
