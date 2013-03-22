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

    private String users = new "http://localhost:8732/moofytest/users/";

    [TestClass]
    public class UnitTest1 {

        static ServiceHost host = null;

        //This should probably be a test-utility class on its own
        //[AssemblyInitialize]
        //public static void InitiateServices(TestContext t) {
        //    Uri baseAddr = new Uri("http://localhost:8732/moofytest");
        //    string addr = "users";

        //    host = new ServiceHost(typeof(MoofyServices), baseAddr);
        //    host.AddServiceEndpoint(typeof(IUserService), new BasicHttpBinding(), addr);
        //    host.Open();

        //}

        //[AssemblyCleanup]
        //public static void KillServices() {
        //    Thread.Sleep(300);
        //    host.Close();
        //}

        //[TestMethod]
        //public void TestMethod1() {
        //    Uri u = new Uri("http://localhost:8732/moofytest/users/1");
        //    WebRequest r = WebRequest.Create(u);
        //    WebResponse res = r.GetResponse();
        //    System.Console.WriteLine(res);
        //}
        [TestMethod]
        public void ValidInUseIdOne(){
            Uri u = new Uri(users+"1"),
            WebRequest r = WebRequest.Create(users);
            WebResponse res = r.GetResponse();
            StreamReader reader = new Streamreader(res.GetResponseStream());
            String conts = reader.ReadToEnd();
            bool isJsonUser = conts.StartsWith("{") &&
                conts.EndsWith("}") &&
                conts.Contains("balance") &&
                conts.Contains("email") &&
                conts.Contains("name") &&
                conts.Contains("username");

            Assert.IsTrue(isJsonUser, conts + " is not a valid user");
        }
    }
}
