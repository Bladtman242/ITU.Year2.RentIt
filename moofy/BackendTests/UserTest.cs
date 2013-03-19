using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace moofy.Backend.Tests {
    /// <summary>
    /// Summary description for UserTest
    /// </summary>
    [TestClass]
    public class UserTest {
        public UserTest() {
            //
            // TODO: Add constructor logic here
            //
        }

        [TestMethod]
        public void basicPropertyTest() {
            User testUser = new User(1) {
                Name = "Test Name",
                Username = "Test Username",
                Password = "Test Password",
                Email = "test@test.com",
                Balance = 150,
            };




            Assert.AreEqual<bool>(true, DBUser.GetUser(1).IsAdmin);

            //Negative test
            Assert.AreEqual<bool>(false, DBUser.GetUser(2).IsAdmin);
        }


        [TestMethod]
        public void IsAdminTest() {
            //Positive test
            Assert.AreEqual<bool>(true, DBUser.GetUser(1).IsAdmin);
            
            //Negative test
            Assert.AreEqual<bool>(false, DBUser.GetUser(2).IsAdmin);
        }

        [TestMethod]
        public void PurchasesTest() {
            IList<Purchase> purchases = new List<Purchase>();

            //Positive test

            //Negative test
            Assert.AreEqual<bool>(false, DBUser.GetUser(2).IsAdmin);
        }
    }
}
