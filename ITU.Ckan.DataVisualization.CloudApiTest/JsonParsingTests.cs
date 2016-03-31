using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Dynamic;
using System.Collections.Generic;
using System.Linq;
using ITU.Ckan.DataVisualization.CloudApi.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis.Emit;
using System.Collections;

namespace ITU.Ckan.DataVisualization.CloudApiTest
{
    [TestClass]
    public class JsonParsingTests
    {
        [TestMethod]
        public void JsonParting()
        {
            //var TestClass = new { EmployeeID = 108, EmployeeName = "John Doe" };

            var records = new Object();
            var dummyObject = new
            {
                help = "",
                sucess = true,
                result = new { records = new Object[10] },
            };

            var rec = dummyObject.result.records;

            //rec = new ExpandoObject();
            //((IDictionary<string, object>)rec)["test"] = new object[10];

            //JsonConvert.DeserializeAnonymousType(data, dummyObject);
        }

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

            //JsonConvert.DeserializeAnonymousType(data, dummyObject);
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

            dynamic dinamycFilters = new RecordDTO();
            dinamycFilters = new ExpandoObject();
            dinamycFilters.Status = "";
            dinamycFilters.bknr = "";

            //var sdf = (newClass as ExpandoObject).ToList();

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

            var sd = new RecordDTO();

            //var MyClass = syntaxRoot.DescendantNodes().OfType<Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax>().First();
            //var MyMethod = syntaxRoot.DescendantNodes().OfType<Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax>().First();
            //var type = tree.GetType();
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

            //Type typeResult = assembly.GetType("ITU.Ckan.DataVisualization.CloudApi.DTO.ResultsDTO");
            //Type typeRecords = assembly.GetType("ITU.Ckan.DataVisualization.CloudApi.DTO.RecordsDTO");
            Type typeRecord = assembly.GetType("ITU.Ckan.DataVisualization.CloudApi.DTO.RecordDTO");
            //object objResult = Activator.CreateInstance(typeResult);
            //object objRecords = Activator.CreateInstance(typeRecords);
            dynamic objRecord = Activator.CreateInstance(typeRecord);


            //var DyObjectsList = new List<RecordDTO>();
            //dynamic DyObj = new ExpandoObject();
            //DyObj._id = 0;
            //DyObj.beslutnings_pjece = "";

            dynamic dinamycFilters = new RecordDTO();
            dinamycFilters = new ExpandoObject();
            dinamycFilters._id = 0;
            dinamycFilters.beslutnings_pjece = "";

            //Type d1 = typeof(List<>);
            //Type[] typeArgs = { typeof(RecordDTO) };
            //Type makeme = d1.MakeGenericType(typeArgs);
            //object o = Activator.CreateInstance(makeme);

            var element = new ResultsDTO();
            element.result = new RecordsDTO();
            element.result.records = null;

            
            
            //var rest =  GenericRestfulClient.GetCkan
            //    ("http://data.kk.dk", "/api/action/datastore_search_sql?sql=SELECT _id,beslutnings_pjece FROM" + @" ""2401bae1-b4c7-4c8a-9bcc-6b5fb9c1dcbc"" ", element);

            /*
            var executionResult = type.InvokeMember("Write",
                BindingFlags.Default | BindingFlags.InvokeMethod,
                null,
                obj,
                new object[] { "Hello World" });
                
            var sdfs = executionResult.ToString();
            */
        }

        [TestMethod]
        public void TestExtendObject()
        {
            dynamic TestClass = new RecordDTO();
            TestClass = new ExpandoObject();
            ((IDictionary<string, object>)TestClass)["_id"] = 0;
            ((IDictionary<string, object>)TestClass)["beslutnings_pjece"] = "";

            Type listType = typeof(List<>).MakeGenericType(TestClass.GetType());
            IList myList = (IList)Activator.CreateInstance(listType);

            var element = new ResultsDTO();
            element.result = new RecordsDTO();
            //element.result.records = myList;

            //var rest = GenericRestfulClient.GetCkan
            //    ("http://data.kk.dk", 
            //    "/api/action/datastore_search_sql?sql=SELECT _id,beslutnings_pjece FROM" + @" ""2401bae1-b4c7-4c8a-9bcc-6b5fb9c1dcbc"" ", 
            //    element);

        }

    }

}
