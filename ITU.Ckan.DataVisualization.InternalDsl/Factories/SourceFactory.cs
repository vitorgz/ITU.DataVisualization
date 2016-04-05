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
