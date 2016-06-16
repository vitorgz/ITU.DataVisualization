using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl.IFactories
{
    public interface ISourceFactory
    {
        Source Create();
        ISourceFactory AddIn(Action<ISourceFactory> action);

        //ISourceFactory Initialize();
        ISourceFactory AddGroup(List<Group> groups);
        ISourceFactory AddOrganization(List<Organization> organizations);
        ISourceFactory AddTag(List<Tag> tags);
        ISourceFactory AddPackage(List<Package> packages);
        

        ISourceFactory GetGroups(string source);
        ISourceFactory GetOrganizations(string source);
        ISourceFactory GetTag(string source);
        ISourceFactory GetPackages(string source);
    }
}
