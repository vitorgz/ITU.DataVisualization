using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ITU.Ckan.DataVisualization.CloudApi.Helpers;
using System.Collections.Generic;
using ITU.Ckan.DataVisualization.CloudApi.Deserialize;
using System.Linq;

namespace ITU.Ckan.DataVisualization.CloudApiTest
{
    [TestClass]
    public class CloudHelpersTest
    {
        [TestMethod]
        public void ConcertionTests()
        {
            var data = new[] { "12", "13", null };
            var arr = CloudApiHelpers.ConvertArrayToSpecificType(data, typeof(int));

            Assert.IsNotNull(arr);
        }

        [TestMethod]
        public void ConvertToType()
        {
            var ld = new ListDeserialize();
            var data = new List<string>() { "test", "test1"};
            ld.result = data;
            var list = CloudApiHelpers.ConvertToListOfType<Tag>(ld);
            
            Assert.IsTrue(list.Any());
        }

        [TestMethod]
        public void PieChartPercentageDataTest()
        {
            var rec = new Record();
            //var data = new List<string>() { "test", "test1", "test", "test", "test", "test", "test", "test1", "test", "test" };
            var data = new List<int>() { 0,0,0,0,0,0,0,0,1,1 };
            rec.value = data;

            var table = CloudApiHelpers.PieChartAnalizeAndCreateTable(rec);

            Assert.IsNotNull(table);
        }
    }
}
