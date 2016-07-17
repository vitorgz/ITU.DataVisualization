using ITU.Ckan.DataVisualization.InternalDslApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.InternalDsl.ExtensionMethods
{
    public static class SourceExtensionMethods
    {
        public static Source GetSource(this Visualization root, string name)
        {
            var s = root.sources.Where(x => x.name == name).FirstOrDefault();
            return s;
        }

        public static Source GetSourceById(this Visualization root, Expression<Func<Source, bool>> property)
        {
            Func<Source, bool> funcWhere = property.Compile();
            var s = root.sources.Where(funcWhere).FirstOrDefault();
            return s;
        }

        public async static Task<List<Group>> GetGroups(this Source root, string source)
        {
            return await InternalClient.GetGroups<List<Group>>(source);
        }

        public async static Task<List<Tag>> GetTags(this Source root, string source)
        {
            return await InternalClient.GetTags<List<Tag>>(source);
        }

        public async static Task<List<Organization>> GetOrganizations(this Source root, string source)
        {
            return await InternalClient.GetOrganizations<List<Organization>>(source);
        }

        public static async Task<List<Package>> GetPackages(this Source root, string id)
        {
            return await InternalClient.GetPackages<List<Package>>(id);
        }
        

        public static void GetPackagesToSource(this Source source, string src)
        {
            var task = Task.Run(async () =>
            {
                var tas = await InternalClient.GetPackages<List<Package>>(src);
                source.packages = tas;
            });
            task.Wait();
        }

        public static void GetGroupsToSource(this Source source, string src)
        {
            var task = Task.Run(async () =>
            {
                var tas = await InternalClient.GetGroups<List<Group>>(src);
                source.groups = tas;
            });
            task.Wait();
        }

        public static void GetOrganizationsToSource(this Source source, string src)
        {
            var task = Task.Run(async () =>
            {
                var tas = await InternalClient.GetOrganizations<List<Organization>>(src);
                source.organizations = tas;
            });
            task.Wait();
        }

        public static void GetTagsToSource(this Source source, string src)
        {
            var task = Task.Run(async () =>
            {
                var tas = await InternalClient.GetTags<List<Tag>>(src);
                source.tags = tas;
            });
            task.Wait();
        }

        public static Source AddIn(this Source source, Action<Source> action)
        {
            action.Invoke(source);

            return source;
        }
    }
}
