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
        DBAccess db;
        public UserTest() {
            db = new DBAccessTest();
        }

        /// <summary>
        /// Tests that the basic properties are set and retrieved correctly.
        [TestMethod]
        public void basicPropertyTest() {
            User firstTestUser = new User(1) {
                Name = "Test Name",
                Username = "Test Username",
                Password = "Test Password",
                Email = "test@test.com",
                Balance = 150,
            };

            Assert.AreEqual<int>(1, firstTestUser.Id);
            Assert.AreEqual<string>("Test Name", firstTestUser.Name);
            Assert.AreEqual<string>("Test Username", firstTestUser.Username);
            Assert.AreEqual<string>("Test Password", firstTestUser.Password);
            Assert.AreEqual<string>("test@test.com", firstTestUser.Email);
            Assert.AreEqual<int>(150, firstTestUser.Balance);


            User otherTestUser = new User(2) {
                Name = "other Name",
                Username = "other Username",
                Password = "other Password",
                Email = "other@test.com",
                Balance = 200,
            };
            Assert.AreEqual<int>(2, otherTestUser.Id);
            Assert.AreEqual<string>("other Name", otherTestUser.Name);
            Assert.AreEqual<string>("other Username", otherTestUser.Username);
            Assert.AreEqual<string>("other Password", otherTestUser.Password);
            Assert.AreEqual<string>("other@test.com", otherTestUser.Email);
            Assert.AreEqual<int>(200, otherTestUser.Balance);
        }

        /// <summary>
        /// Tests whether the Admin boolean is retrieved correctly in the user object
        /// </summary>
        [TestMethod]
        public void IsAdminTest() {
            //Positive test
            Assert.AreEqual<bool>(true, db.GetUser(1).IsAdmin);
            
            //Negative test
            Assert.AreEqual<bool>(false, db.GetUser(2).IsAdmin);
        }

        /// <summary>
        /// Tests whether the purchases are retrieved correctly in the user object
        /// </summary>
        [TestMethod]
        public void PurchasesTest() {
            //Cannot be implemented without some test data
            //awaiting the DB classes
            Assert.Fail("awaiting the DB classes");
        }
    }
}
