using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl.Factories
{
    public class SourceFactory : ISourceFactory
    {
        public ISourceFactory Initialize()
        {
            throw new NotImplementedException();
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

        
    }
}
