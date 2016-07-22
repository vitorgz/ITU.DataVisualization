using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ITU.Ckan.DataVisualization.InternalDsl;
using System.Collections.Generic;
using System.Linq;
using ITU.Ckan.DataVisualization.InternalDsl.Factories;
using ITU.Ckan.DataVisualization.InternalDsl.ExtensionMethods;
using System.Linq.Expressions;

namespace ITU.Ckan.DataVisualization.MetaMetaModelTest
{
    [TestClass]
    public class ModelTest
    {
        [TestMethod]
        public void AddNameAndGroupToASource()
        {
            var source = GenericFluentFactory<Source>
            .Init(new Source())
                .AddGenericProperty(x => x.name, "TestSource")
                .AddGenericProperty(x => x.groups, new List<Group>() {
                    new Group() { properties = 
                    new List<Property>() {
                        new Property() { name = "GroupTest" }
                    } } })
            .Create();

            Assert.IsNotNull(source.groups);
            Assert.IsTrue(source.groups.Any());
        }

        [TestMethod]
        public void AddTableToVisualization()
        {
            var visual = GenericFluentFactory<Visualization>
            .Init(new Visualization()).AddIn(x=> {

                x.GetSource("http://data.kk.dk");
                x.AddTable(new Table());
                x.name = "test";
            }).Create();

            Assert.IsNotNull(visual.table);
        }

        [TestMethod]
        public void AddNameAndGroupToSource()
        {
            var source = GenericFluentFactory<Source>
            .Init(new Source())
                .AddGenericProperty(x => x.name, "blabla")
                .AddGenericProperty(x => x.groups, new List<Group>() { new Group() { properties = new List<Property>() { new Property() { name = "test" } } } })
            .Create();

            Assert.IsNotNull(source.groups);
            Assert.IsTrue(source.groups.Any());
        }


        [TestMethod]
        public void CheckLinqisWorking()
        {
            var source = GenericFluentFactory<Source>
            .Init(new Source())
                .AddGenericProperty(x => x.name, "blabla")
                .AddGenericProperty(x => x.groups, new List<Group>() { new Group() { properties = new List<Property>() { new Property() { name = "test" } } } })
            .Create();

            var prop = source.groups.Select(x => x.properties);

            Assert.IsNotNull(prop);
            Assert.IsTrue(source.groups.Any());
        }

        [TestMethod]
        public void AddNameAndpToASource()
        {
            var prop = new Property() { name = "TET", value = 12 };

            var source = GenericFluentFactory<Source>
            .Init(new Source())
                .AddGenericProperty(x => x.name, "blabla")
                .AddGenericProperty(x => x.packages, new List<Package>() { new Package() { properties = new List<Property>() { prop } } })
            .Create();

            Assert.IsNotNull(source.groups);
            Assert.IsTrue(source.groups.Any());
        }

        [TestMethod]
        public void TestPropertiesInterface()
        {
            var tag = new Tag().AddGenericProperties("Author", "Vitor Dominguez");
            tag.AddGenericProperties("state", "active");

            var ss = tag;
        }

        [TestMethod]
        public void TestRootFactory()
        {
            var r = new Root();
            r.visualizations.Add(new Visualization() { name = "test" });

            var s = r.GetVisualization("test");
            var ss = r.GetVisualizationById(x => x.name == "test");
        }

        [TestMethod]
        public void TestDataSetFactory()
        {
            var ds = new DataSetFactory()
                .AddField(new List<Field>())
                .AddGroup(new List<Group>())
                .AddOrganization(new Organization())
                .Create();

            Assert.IsNotNull(ds);
        }

        [TestMethod]
        public void TestAddInDSL()
        {
            var mySource =
              new SourceFactory().AddIn(
               x =>
               {
                   x.AddGroup(new List<Group>() { new Group() { name = "test" } }); 
                   x.AddTag(new List<Tag>() { new Tag() { name = "testTAg" } });

                   //or
                   //x.AddGroup(new List<Group>() { new Group() { name = "test" } }).AddTag(new List<Tag>() { new Tag() { name = "testTAg" } });
               }
               ).Create();

            Assert.IsNotNull(mySource);
        }

        [TestMethod]
        public void TestAddInGetsDSL()
        {
            var mySource =
             new SourceFactory().AddIn(
               x =>
               {
                   x.GetGroups("tets");
                   x.GetOrganizations("test");
                   x.GetPackages("test");
                 }
               ).Create();

            Assert.IsNotNull(mySource);
        }

        [TestMethod]
        public void CheckVisualizationDSLMethods()
        {
            var visual = new Visualization();
            visual = visual.AddIn(x =>
            {
                x.AddTable(new Table() { name = "test"});
            });

            Assert.IsNotNull(visual);
        }

        [TestMethod]
        public void TestAddInDSL2()
        {
            var mySource =
              new SourceFactory().AddIn(
               x =>
               {
                   x.GetGroups("http://data.kk.dk");
                   x.GetTag("http://data.kk.dk");
               }
               ).Create();

            Assert.IsNotNull(mySource);
        }

        [TestMethod]
        public void TestAddInDSL3()
        {
            var visual = new Visualization();

            //option 1 - standard
            visual.table = new Table();

            //option 2 - Literal extensions (extension method)
            visual.AddTable(new Table());                      

            //option 3 - Closures && Nested Closures
            visual.AddIn(
               x =>
               {
                   x.AddIn(y => y.table, new Table());
                   x.AddIn(y => y.name, "test");
               });
                        
            Assert.IsNotNull(visual.table);
        }

        [TestMethod]
        public void TestExtensionMethodSource()
        {
            var source = new Source();

            source.AddIn(
               x =>
               {
                   x.GetTagsToSource("test");
               });

            Assert.IsNotNull(source);

        }
    }
}
