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
    }
}
