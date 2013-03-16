using moofy.JsonServices;
using System;
using System.Net;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace moofy.JsonServices.Tests {
    
    [TestClass]
    public class UnitTest1 {

        ServiceHost host = null;

        //This should probably be a test-utility class on its own
        [AssemblyInitialize]
        public static void InitiateServices(TestContext t) {
            Uri baseAddr = new Uri("http://localhost:8732/moofytest");

            using (ServiceHost host = new ServiceHost(typeof(MoofyServices), baseAddr)) {
                host.Open();
            }
        }

        [TestMethod]
        public void TestMethod1() {
            
        }
    }
}
