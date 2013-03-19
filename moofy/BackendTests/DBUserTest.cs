using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace moofy.Backend.Tests {
    /// <summary>
    /// Summary description for DBUserTest
    /// </summary>
    [TestClass]
    public class DBUserTest {
        public DBUserTest() {
            //
            // TODO: Add constructor logic here
            //
        }

        [TestMethod]
        public void GetUserTest() {
            User user = DBUser.GetUser(1);
            Console.WriteLine("HEHEHEHEHE" + user.Name);
            Assert.AreEqual<string>("jegerdum", user.Password);
        }
    }
}
