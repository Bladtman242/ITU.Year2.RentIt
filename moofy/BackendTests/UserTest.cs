using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace moofy.Backend.Tests {
    /// <summary>
    /// Summary description for UserTest
    /// </summary>
    [TestClass]
    public class UserTest {
        static DBAccess db;

        public UserTest() {
            db = new DBAccess();
            db.Open();
        }

        [ClassCleanup()]
        public static void CleanUp() {
            db.Close();
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
            ////Upload a file
            //MemoryStream s = new MemoryStream();
            //s.WriteByte(5);
            //int tmpId = db.UploadFile(s);
            ////Create a movie
            //int firstMovieId = db.CreateMovie(1, tmpId, "testmovie1", 1950, 100, 10, "testest", new string[] { "Horror" }, "description1");
            

            ////Upload another file
            //s = new MemoryStream();
            //s.WriteByte(5);
            //tmpId = db.UploadFile(s);
            ////Create another movie to buy with price 100
            //int secondMovieId = db.CreateMovie(1, tmpId, "testmovie2", 1980, 200, 30, "testest", new string[] { "Action" }, "description2");
            
            ////Create a user to purchase the movies
            //int userId = db.AddUser(new User(0) {
            //    Name = "TestUser",
            //    Username = "TestUser",
            //    Balance = 200,
            //    Email = "testuser@test.com",
            //    Password = "test"
            //});

            //db.PurchaseMovie(firstMovieId, userId);
            //db.PurchaseMovie(secondMovieId, userId);

            ////Get the user from the database
            //User actualUser = db.GetUser(userId);


            ////Assert that the movie is added to movies owned by the user
            //Assert.IsTrue(actualUser.Purchases.ElementAt(0).File.Id == movieId);
            ////Assert that the date of expiration is set to max, comparing strings as DateTimes compare is an reference equality
            //Assert.AreEqual(actualUser.Purchases.ElementAt(0).EndTime.ToString(), DateTime.MaxValue.ToString());

            //db.DeleteUser(userId);
            //db.DeleteMovie(firstMovieId, 1);
            //db.DeleteMovie(firstMovieId, 1);
            ////Cannot be implemented without some test data
            ////awaiting the DB classes
            Assert.Fail("awaiting the DB classes");
        }
    }
}
