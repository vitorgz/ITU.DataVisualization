using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Dynamic;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis.Emit;
using System.Collections;
using ITU.Ckan.DataVisualization.CloudApi.Helpers;
using System.Drawing;

namespace ITU.Ckan.DataVisualization.CloudApiTest
{
    [TestClass]
    public class JsonParsingTests
    {

        [TestMethod]
        public void DinamicallyCreateClass()
        {
            //var TestClass = new { EmployeeID = 108, EmployeeName = "John Doe" };
            var TestClass = new object();

            TestClass = new ExpandoObject();
            ((IDictionary<string, object>)TestClass)["id"] = 0;
            ((IDictionary<string, object>)TestClass)["name"] = "test";

            var s = TestClass;

            Assert.IsNotNull(s);

        }

        [TestMethod]
        public void DeserializeDinamicObject()
        {
            string myJson = "{ help:'', success: true, result: { records: [{ status: 'Vedtaget byfornyelsesbeslutning',  bknr: '1164'}, {status: 'Vedtaget byfornyelsesbeslutning', bknr: '1167'}]}}";

            var filters = new List<string>() { "status", "bknr" };
            var newClass = new object();
            newClass = new ExpandoObject();
            foreach (var item in filters)
            {
                ((IDictionary<string, object>)newClass)[item] = item.GetType().TypeInitializer;
            }

            dynamic employee = new ExpandoObject();
            employee.Name = "John Smith";
            employee.Age = 33;

            var sd = (newClass as ExpandoObject).ToList();
            var sdf = new { status = "", bknr = "" };

            var dummyObject = new
            {
                help = "",
                sucess = true,
                result = new { records = new[] { sdf } },
            };

            var myObjects = JsonConvert.DeserializeAnonymousType(myJson, dummyObject);
            Console.WriteLine(myObjects);
        }

        [TestMethod]
        public void DeserializeAnonymousType()
        {
            //http://stackoverflow.com/questions/3142495/deserialize-json-into-c-sharp-dynamic-object/3806407#3806407
            string json = "{ help:'', success: true, result: { records: [{ status: 'Vedtaget byfornyelsesbeslutning',  bknr: '1164'}, {status: 'Vedtaget byfornyelsesbeslutning', bknr: '1167'}]}}";

            //dynamic data = Json.Decode(json);
            var data = JsonConvert.DeserializeObject(json);
            var data2 = JObject.Parse(json);
        }

        [TestMethod]
        public void DeserializeAsDictionary()
        {
            string json = "[{ status: 'Vedtaget byfornyelsesbeslutning',  bknr: '1164'}, {status: 'Vedtaget byfornyelsesbeslutning', bknr: '1167'}]";

            List<Dictionary<string, string>> htmlAttributes = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(json);

            Assert.IsTrue(htmlAttributes.Any());

        }

        [TestMethod]
        public void DeserializeListUsingLinq()
        {
            string json = "{ help:'', success: true, result: { records: [{ status: 'Vedtaget byfornyelsesbeslutning',  bknr: '1164'}, {status: 'Vedtaget byfornyelsesbeslutning', bknr: '1167'}]}}";

            JObject data = JObject.Parse(json);

            //string rssTitle = (string)data["result"]["records"][0];

            var postTitles = from p in data["result"]["records"]
                             select (string)p["status"];

            Assert.IsTrue(postTitles.Any());
        }

        [TestMethod]
        public void RoslynCreateClassTest()
        {
            string json = "{ help:'', success: true, result: { records: [{ status: 'Vedtaget byfornyelsesbeslutning',  bknr: '1164'}, {status: 'Vedtaget byfornyelsesbeslutning', bknr: '1167'}]}}";


            var tree = CSharpSyntaxTree.ParseText(@"
             namespace ITU.Ckan.DataVisualization.CloudApi.DTO
                { 
                public class RecordDTO
                    {
                        public string status{get;set;}
                    } } ");


            var syntaxRoot = tree.GetRoot();

            Assert.IsNotNull(syntaxRoot);
        }

        [TestMethod]
        public void RoslynCreateAndUserClassTest()
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(@"
                                            using System;
                                            using System.Collections.Generic;

                                            namespace ITU.Ckan.DataVisualization.CloudApi.DTO
                                            {                                                 
                                                public class RecordDTO
                                                {
                                                    public string _id {get;set;}
                                                } 
                                            } ");

            string assemblyName = Path.GetRandomFileName();
            MetadataReference[] references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
            };

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            Assembly assembly = null;
            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in failures)
                    {
                        Console.Error.WriteLine("{ 0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    assembly = Assembly.Load(ms.ToArray());
                }
            }

            Type typeRecord = assembly.GetType("ITU.Ckan.DataVisualization.CloudApi.DTO.RecordDTO");
            dynamic objRecord = Activator.CreateInstance(typeRecord);

            Assert.IsNotNull(objRecord);

        }

        [TestMethod]
        public void ProcessJson()
        {
            string json = "{ help:'', success: true, result: { records: [{ status: 'Vedtaget byfornyelsesbeslutning',  bknr: 1164}, {status: 'Vedtaget byfornyelsesbeslutning', bknr: 1167}]}}";

            var fields = new List<Field>() { new Field() { id = "status", type= typeof(string) }, new Field() { id = "bknr", type = typeof(int) } };
            CloudApiHelpers.ProcessJsonResponse(json, fields);

            Assert.IsNotNull(fields);

        }

        [TestMethod]
        public void ProcessCheckNullsJson()
        {
            string json = "{ help:'', success: true, result: { records: [{ status: 'Vedtaget byfornyelsesbeslutning',  bknr: null}, {status: 'Vedtaget byfornyelsesbeslutning', bknr: 1167}]}}";

            var fields = new List<Field>() { new Field() { id = "status", type = typeof(string) }, new Field() { id = "bknr", type = typeof(int) } };
            CloudApiHelpers.ProcessJsonResponse(json, fields);

            Assert.IsNotNull(fields);

        }

        [TestMethod]
        public void ProcessPointsJson()
        {
            string json = "{ help:'', success: true, result: { records: [{ status: 'Vedtaget byfornyelsesbeslutning',  wkb_geometry: 'POINT(12.572665592562222 55.67900255632159)'}, {status: 'Vedtaget byfornyelsesbeslutning', wkb_geometry: 'POINT(12.555178540169926 55.681753235913526)'}]}}";

            var fields = new List<Field>() {
                new Field() { id = "status", type = typeof(string) },
                new Field() { id = "wkb_geometry", type = typeof(Point) } };
            CloudApiHelpers.ProcessJsonResponse(json, fields);

            Assert.IsNotNull(fields);

        }

    }

}
