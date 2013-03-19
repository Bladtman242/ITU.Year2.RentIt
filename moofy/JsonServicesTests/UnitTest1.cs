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

        static ServiceHost host = null;

        //This should probably be a test-utility class on its own
        [AssemblyInitialize]
        public static void InitiateServices(TestContext t) {
            Uri baseAddr = new Uri("http://localhost:8732/moofytest");
            string addr = "users";

            host = new ServiceHost(typeof(MoofyServices), baseAddr);
            host.AddServiceEndpoint(typeof(IUserService), new BasicHttpBinding(), addr);
            host.Open();

        }

        [AssemblyCleanup]
        public static void KillServices() {
            Thread.Sleep(300);
            host.Close();
        }

        [TestMethod]
        public void TestMethod1() {
            Uri u = new Uri("http://localhost:8732/moofytest/users/1");
            WebRequest r = WebRequest.Create(u);
            WebResponse res = r.GetResponse();
            System.Console.WriteLine(res);
        }
    }
}
