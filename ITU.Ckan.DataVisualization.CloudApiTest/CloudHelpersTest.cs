using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ITU.Ckan.DataVisualization.CloudApi.Helpers;

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
    }
}
