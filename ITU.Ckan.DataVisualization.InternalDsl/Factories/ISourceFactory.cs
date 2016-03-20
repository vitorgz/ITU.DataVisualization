using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl.Factories
{
    public interface ISourceFactory
    {
        ISourceFactory Initialize();
        ISourceFactory AddGroup(List<Group> groups);
        ISourceFactory AddOrganization(List<Organization> organizations);
        ISourceFactory AddTag(List<Tag> tags);
        ISourceFactory AddPackage(List<Package> packages);
        ISourceFactory GetSources();
        Task<ISourceFactory> GetPackages(string source);
        Source Create();
    }
}
