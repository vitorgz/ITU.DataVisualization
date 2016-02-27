using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ITU.Ckan.DataVisualization.InternalDsl;
using System.Collections.Generic;
using System.Linq;
using ITU.Ckan.DataVisualization.InternalDsl.Factories;

namespace ITU.Ckan.DataVisualization.MetaMetaModelTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AddNameAndGroupToASource()
        {
            var source = GenericFluentFactory<Source>
            .Init(new Source())
            .AddPropertyValue(x => x.name, "blabla")
            .AddPropertyValue(x => x.groups, new List<Group>() { new Group() { properties = new List<Property>() { new Property() { name = "test" } } } })
            .Create();

            Assert.IsNotNull(source.groups);
            Assert.IsTrue(source.groups.Any());
        }

        [TestMethod]
        public void CheckLinqisWorking()
        {
            var source = GenericFluentFactory<Source>
            .Init(new Source())
            .AddPropertyValue(x => x.name, "blabla")
            .AddPropertyValue(x => x.groups, new List<Group>() { new Group() { properties = new List<Property>() { new Property() { name = "test" } } } })
            .Create();

            var prop = source.groups.Select(x => x.properties);

            Assert.IsNotNull(prop);
            Assert.IsTrue(source.groups.Any());
        }

        [TestMethod]
        public void AddNameAndpToASource()
        {
            //var prop = GenericFluentFactory<Property>
            //    .Init(new Property())
            //    .AddPropertyValue(x => x.Name, "test")
            //    .AddPropertyValue(x => x.Value, 123)
            //    .Create();

            var prop = new Property() { name = "TET", value = 12 };

            var source = GenericFluentFactory<Source>
            .Init(new Source())
            .AddPropertyValue(x => x.name, "blabla")
            .AddPropertyValue(x => x.packages, new List<Package>() { new Package() { properties = new List<Property>() { prop } } })
            .Create();

            Assert.IsNotNull(source.groups);
            Assert.IsTrue(source.groups.Any());
        }

        [TestMethod]
        public void TestPropertiesInterface()
        {
            //var s = new Organization().WithProperties<Organization>("test");
            var s = new Tag().WithProperties("test");

            var ss = s;
        }

        [TestMethod]
        public void TestAttributesInterface()
        {
            //var s = new Organization().WithProperties<Organization>("test");

            var s = new Source()
                .addAttributes(new List<Group>())
                .addAttributes(new List<Package>());

            //var sd = s.Packages.Where(x => x.DataSets.FirstOrDefault().addFields(new List<Field>()));

            Assert.IsNotNull(s);
        }

        [TestMethod]
        public void TestRootFactory()
        {
            var r = new RootFactory().Initialize()
                .AddVisualization("test").Create();

            var s = r.GetVisualization("test");
            var ss = r.GetVisualizationById(x => x.name == "test");

            var ta = ss.addAttributes(new List<Group>());
        }

        public void TestDataSetFactory()
        {
            var ds = new DataSetFactory().Initialize()
                .AddField(null)
                .AddGroup(null)
                .AddOrganization(null)
                .Create();
            
        }
    }
}
