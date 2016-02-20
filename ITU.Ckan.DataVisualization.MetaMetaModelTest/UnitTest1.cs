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
            .AddPropertyValue(x => x.Name, "blabla")
            .AddPropertyValue(x => x.Groups, new List<Group>() { new Group() { Properties = new List<Property>() { new Property() { Name = "test" } } } })
            .Create();

            Assert.IsNotNull(source.Groups);
            Assert.IsTrue(source.Groups.Any());
        }

        [TestMethod]
        public void CheckLinqisWorking()
        {
            var source = GenericFluentFactory<Source>
            .Init(new Source())
            .AddPropertyValue(x => x.Name, "blabla")
            .AddPropertyValue(x => x.Groups, new List<Group>() { new Group() { Properties = new List<Property>() { new Property() { Name = "test" } } } })
            .Create();

            var prop = source.Groups.Select(x => x.Properties);

            Assert.IsNotNull(prop);
            Assert.IsTrue(source.Groups.Any());
        }

        [TestMethod]
        public void AddNameAndpToASource()
        {
            //var prop = GenericFluentFactory<Property>
            //    .Init(new Property())
            //    .AddPropertyValue(x => x.Name, "test")
            //    .AddPropertyValue(x => x.Value, 123)
            //    .Create();

            var prop = new Property() { Name = "TET", Value = 12 };

            var source = GenericFluentFactory<Source>
            .Init(new Source())
            .AddPropertyValue(x => x.Name, "blabla")
            .AddPropertyValue(x => x.Packages, new List<Package>() { new Package() { Properties = new List<Property>() { prop } } })
            .Create();

            Assert.IsNotNull(source.Groups);
            Assert.IsTrue(source.Groups.Any());
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
                .AddSource("test").Create();

            var s = r.GetSource("test");
            var ss = r.GetSourceById(x => x.Name == "test");

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
