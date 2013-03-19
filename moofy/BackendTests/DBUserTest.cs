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
            //Positive test
            //Expected user
            User expectedUser = new User(1) {
                Name = "Niclas Benjamin Tollstorff",
                Username = "SmallSon",
                Balance = -500,
                Email = "nben@itu.dk",
                Password = "jegerdum"
            };

            User actualUser = DBUser.GetUser(1);
            Assert.AreEqual<int>(expectedUser.Id, actualUser.Id);
            Assert.AreEqual<string>(expectedUser.Name, actualUser.Name);
            Assert.AreEqual<string>(expectedUser.Username, actualUser.Username);
            Assert.AreEqual<int>(expectedUser.Balance, actualUser.Balance);
            Assert.AreEqual<string>(expectedUser.Email, actualUser.Email);
            Assert.AreEqual<string>(expectedUser.Password, actualUser.Password);

            //Negative tests
            User otherUser = DBUser.GetUser(2); //Other user with completely different data
            Assert.AreNotEqual<int>(otherUser.Id, actualUser.Id);
            Assert.AreNotEqual<string>(otherUser.Name, actualUser.Name);
            Assert.AreNotEqual<string>(otherUser.Username, actualUser.Username);
            Assert.AreNotEqual<int>(otherUser.Balance, actualUser.Balance);
            Assert.AreNotEqual<string>(otherUser.Email, actualUser.Email);
            Assert.AreNotEqual<string>(otherUser.Password, actualUser.Password);

            //No user
            User nonExistingUser = DBUser.GetUser(-2);
            Assert.AreEqual<User>(null, nonExistingUser);
        }
    }
}
