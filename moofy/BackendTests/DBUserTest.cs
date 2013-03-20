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
        public DBUserTest() {
            //
            // TODO: Add constructor logic here
            //
        }
        /// <summary>
        /// Used for cleaning up during and after testing. Deletes a user with a given ID.
        /// </summary>
        

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

        [TestMethod]
        public void AddUserTest()
        {
            //User to add. Note Id is not given here but generated and returned in the AddUser method
            User expectedUser = new User(0)
            {
                Name = "AddTestUser",
                Username = "AddTestUser",
                Balance = 10,
                Email = "testuser@add.com",
                Password = "test"
            };

            int givenId = DBUser.AddUser(expectedUser);
            Console.WriteLine(givenId);
            User actualUser = DBUser.GetUser(givenId);

            //Check that the user added to the database is the same as returned by the get method for the givenId
            Assert.AreEqual<int>(givenId, actualUser.Id);
            Assert.AreEqual<string>(expectedUser.Name, actualUser.Name);
            Assert.AreEqual<string>(expectedUser.Username, actualUser.Username);
            Assert.AreEqual<int>(expectedUser.Balance, actualUser.Balance);
            Assert.AreEqual<string>(expectedUser.Email, actualUser.Email);
            Assert.AreEqual<string>(expectedUser.Password, actualUser.Password);

            //Cleanup
            DBUser.deleteUser(givenId);
        }

        [TestMethod]
        public void LoginTest()
        {
            //Adding a user with easy testable login information.
            User expectedUser = new User(0)
            {
                Name = "LoginTestUser",
                Username = "Login",
                Balance = 100,
                Email = "testuser@login.com",
                Password = "test"
            };

            int id = DBUser.AddUser(expectedUser);

            //Test that the right ID of the user is returned
            Assert.AreEqual(DBUser.Login("Login","test"), id);
            //Test that -1 is returned for a user which does not exist
            Assert.AreEqual(DBUser.Login("XXXXXXXXOOOOOOllIIIlllIIaaaaAAAAaadd", "kkieklaklmcmmenns"), -1);

            //Cleanup
            DBUser.deleteUser(id);
        }

        [TestMethod]
        public void DepositTest()
        {
            //Adding a user with a known balance which is easy to test against.
            int balance = 100;
            int deposit = 50;
            User expectedUser = new User(0)
            {
                Name = "DepositTestUser",
                Username = "Deposit",
                Balance = balance,
                Email = "testuser@deposit.com",
                Password = "test"
            };
            int id = DBUser.AddUser(expectedUser);
            bool flag = DBUser.Deposit(deposit, id);
            User actualUser = DBUser.GetUser(id);

            if (flag) Assert.AreEqual(balance + deposit, actualUser.Balance);
        }

        [TestMethod]
        public void PromotetoAdminTest()
        {
            User user = new User(0)
            {
                Name = "AdminTestUser",
                Username = "Admin",
                Balance = 5,
                Email = "testadmin@admin.com",
                Password = "test"
            };

            int promoteeId = DBUser.AddUser(user);
            int promoterId = 1;//predefined admin in the db

            bool flag = DBUser.PromotetoAdmin(promoterId, promoteeId);

            Assert.IsTrue(DBUser.getIsAdmin(promoteeId));

            //clean up
            DBUser.DemoteAdmin(promoterId, promoteeId);
        }

        [TestMethod]
        public void DemoteAdminTest()
        {
            User user = new User(0)
            {
                Name = "AdminTestUser",
                Username = "Admin",
                Balance = 5,
                Email = "testadmin@admin.com",
                Password = "test"
            };

            int demoteeId = DBUser.AddUser(user);
            int demoterId = 1;//predefined admin in the db

            //Promote admin 
            DBUser.PromotetoAdmin(demoterId, demoteeId);

            //Demote and assert
            DBUser.DemoteAdmin(demoterId, demoteeId);

            Assert.IsFalse(DBUser.getIsAdmin(demoteeId));
        }
    }
}
