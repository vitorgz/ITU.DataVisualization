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
            return source;
        }

        public async Task<ISourceFactory> GetPackages(string source)
        {
            var sources = await InternalClient.GetPackages<List<Package>>(source);
            this.source.packages = sources;

            return this;
        }

        public async Task<ISourceFactory> GetGroups(string source)
        {
            var groups = await InternalClient.GetPackages<List<Group>>(source);
            this.source.groups = groups;

            return this;
        }

        public async Task<ISourceFactory> GetOrganizations(string source)
        {
            var orga = await InternalClient.GetPackages<List<Organization>>(source);
            this.source.organizations = orga;

            return this;
        }

        public async Task<ISourceFactory> GetTag(string source)
        {
            var tags = await InternalClient.GetPackages<List<Tag>>(source);
            this.source.tags = tags;

            return this;
        }
    }
}
