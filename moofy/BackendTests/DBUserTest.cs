using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace moofy.Backend.Tests {
    /// <summary>
    /// Summary description for DBUserTest
    /// </summary>
    [TestClass]
    public class DBUserTest {
        static DBAccess db;

        public DBUserTest() {
            db = new DBAccess();
            db.Open();
        }

        [ClassCleanup()]
        public static void CleanUp() {
            db.Close();
        }

        [TestMethod]
        public void GetUserTest() {
            //Positive test
            //Expected user
            User expectedUser = new User() {
                Id = 1,
                Name = "Niclas Benjamin Tollstorff",
                Username = "SmallSon",
                Balance = -500,
                Email = "nben@itu.dk",
                Password = "jegerdum"
            };

            User actualUser = db.GetUser(1);
            Assert.AreEqual<int>(expectedUser.Id, actualUser.Id);
            Assert.AreEqual<string>(expectedUser.Name, actualUser.Name);
            Assert.AreEqual<string>(expectedUser.Username, actualUser.Username);
            Assert.AreEqual<int>(expectedUser.Balance, actualUser.Balance);
            Assert.AreEqual<string>(expectedUser.Email, actualUser.Email);
            Assert.AreEqual<string>(expectedUser.Password, actualUser.Password);

            //Negative tests
            User otherUser = db.GetUser(2); //Other user with completely different data
            Assert.AreNotEqual<int>(otherUser.Id, actualUser.Id);
            Assert.AreNotEqual<string>(otherUser.Name, actualUser.Name);
            Assert.AreNotEqual<string>(otherUser.Username, actualUser.Username);
            Assert.AreNotEqual<int>(otherUser.Balance, actualUser.Balance);
            Assert.AreNotEqual<string>(otherUser.Email, actualUser.Email);
            Assert.AreNotEqual<string>(otherUser.Password, actualUser.Password);

            //No user
            User nonExistingUser = db.GetUser(-2);
            Assert.AreEqual<User>(null, nonExistingUser);
        }

        [TestMethod]
        public void AddUserTest() {
            //User to add. Note Id is not given here but generated and returned in the AddUser method
            User expectedUser = new User() {
                Name = "AddTestUser",
                Username = "AddTestUser",
                Balance = 10,
                Email = "testuser@add.com",
                Password = "test"
            };

            User actualUser = db.AddUser(expectedUser);

            //Check that the user added to the database is the same as returned by the get method for the givenId
            Assert.AreEqual<int>(expectedUser.Id, actualUser.Id);
            Assert.AreEqual<string>(expectedUser.Name, actualUser.Name);
            Assert.AreEqual<string>(expectedUser.Username, actualUser.Username);
            Assert.AreEqual<int>(expectedUser.Balance, actualUser.Balance);
            Assert.AreEqual<string>(expectedUser.Email, actualUser.Email);
            Assert.AreEqual<string>(expectedUser.Password, actualUser.Password);

            //Cleanup
            db.DeleteUser(actualUser.Id);
        }

        [TestMethod]
        public void LoginTest() {
            //Adding a user with easy testable login information.
            User expectedUser = new User() {
                Name = "LoginTestUser",
                Username = "Login",
                Balance = 100,
                Email = "testuser@login.com",
                Password = "test"
            };

            User user = db.AddUser(expectedUser);
            Assert.AreNotEqual<int>(-1, user.Id);

            //Test that the right ID of the user is returned
            Assert.AreEqual<int>(db.Login("Login", "test"), user.Id);
            //Test that -1 is returned for a user which does not exist
            Assert.AreEqual<int>(db.Login("XXXXXXXXOOOOOOllIIIlllIIaaaaAAAAaadd", "kkieklaklmcmmenns"), -1);

            //Cleanup
            db.DeleteUser(user.Id);
        }

        [TestMethod]
        public void DepositTest() {
            //Adding a user with a known balance which is easy to test against.
            int balance = 100;
            int deposit = 50;
            User expectedUser = new User() {
                Name = "DepositTestUser",
                Username = "Deposit",
                Balance = balance,
                Email = "testuser@deposit.com",
                Password = "test"
            };
            User user = db.AddUser(expectedUser);
            bool flag = db.Deposit(deposit, user.Id);
            User actualUser = db.GetUser(user.Id);

            if (flag) Assert.AreEqual(balance + deposit, actualUser.Balance);

            //Cleanup
            db.DeleteUser(user.Id);
        }

        [TestMethod]
        public void PromotetoAdminTest() {
            User user = new User() {
                Name = "AdminTestUser",
                Username = "Admin",
                Balance = 5,
                Email = "testadmin@admin.com",
                Password = "test"
            };

            User promotee = db.AddUser(user);
            int promoterId = 1;//predefined admin in the db

            bool flag = db.PromotetoAdmin(promoterId, promotee.Id);

            Assert.IsTrue(db.GetIsAdmin(promotee.Id));

            //clean up
            db.DemoteAdmin(promoterId, promotee.Id);
            db.DeleteUser(promotee.Id);
        }

        [TestMethod]
        public void DemoteAdminTest() {
            User user = new User() {
                Name = "AdminTestUser",
                Username = "Admin",
                Balance = 5,
                Email = "testadmin@admin.com",
                Password = "test"
            };

            User demotee = db.AddUser(user);
            int demoterId = 1;//predefined admin in the db

            //Promote admin 
            db.PromotetoAdmin(demoterId, demotee.Id);

            //Demote and assert
            db.DemoteAdmin(demoterId, demotee.Id);

            Assert.IsFalse(db.GetIsAdmin(demotee.Id));

            //Cleanup
            db.DeleteUser(demotee.Id);
        }
    }
}
