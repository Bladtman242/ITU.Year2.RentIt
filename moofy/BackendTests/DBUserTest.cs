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
            db.ClearDatabase(); //Clears the database, and initialises it to a test state
        }

        [ClassCleanup()]
        public static void CleanUp() {
            db.ClearDatabase(); //Clears the database, and initialises it to a test state
            db.Close();
        }

        private static void _AssertUserEquality(User expected, User actual) {
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.Password, actual.Password);
            Assert.AreEqual(expected.Email, actual.Email);
            Assert.AreEqual(expected.Balance, actual.Balance);
            Assert.AreEqual(expected.IsAdmin, actual.IsAdmin);
            Assert.AreEqual(expected.Movies.Count, actual.Movies.Count);
            for (int i = 0; i < expected.Movies.Count; i++) {
                Assert.IsTrue(expected.Movies[i].File.Title.Equals(actual.Movies[i].File.Title));
                Assert.IsTrue(expected.Movies[i].EndTime.Equals(actual.Movies[i].EndTime));
            }
            Assert.AreEqual(expected.Songs.Count, actual.Songs.Count);
            for (int i = 0; i < expected.Songs.Count; i++) {
                Assert.IsTrue(expected.Songs[i].File.Title.Equals(actual.Songs[i].File.Title));
                Assert.IsTrue(expected.Songs[i].EndTime.Equals(actual.Songs[i].EndTime));
            }
        }

        //Update User tests
        [TestMethod]
        public void UpdateUserSameValuesTest() {
            User expectedUser = db.GetUser(1);
            Assert.IsTrue(db.UpdateUser(expectedUser));
            User actualUser = db.GetUser(1);
            _AssertUserEquality(expectedUser, actualUser);
        }

        [TestMethod]
        public void UpdateUserChangedNameTest() {
            User expectedUser = db.GetUser(1);
            expectedUser.Name = "Phillip De Romanero";
            Assert.IsTrue(db.UpdateUser(expectedUser));
            User actualUser = db.GetUser(1);
            _AssertUserEquality(expectedUser, actualUser);
        }

        [TestMethod]
        public void UpdateUserChangedUsernameTest() {
            User expectedUser = db.GetUser(1);
            expectedUser.Username = "El Luchador";
            Assert.IsTrue(db.UpdateUser(expectedUser));
            User actualUser = db.GetUser(1);
            Assert.AreNotEqual(expectedUser.Username, actualUser.Username);
        }

        [TestMethod]
        public void UpdateUserInvalidIdTest() {
            User expectedUser = db.GetUser(1);
            expectedUser.Id = -1;
            Assert.IsFalse(db.UpdateUser(expectedUser));
        }

        [TestMethod]
        public void UpdateUserNonExistingTest() {
            User expectedUser = db.GetUser(1);
            expectedUser.Id = 20;
            Assert.IsFalse(db.UpdateUser(expectedUser));
        }

        //Delete User
        [TestMethod]
        public void DeleteUserValidIdTest() {
            User expectedUser = db.GetUser(3);
            Assert.IsTrue(db.DeleteUser(expectedUser.Id));
            Assert.IsTrue(db.GetUser(3) == null);
        }

        [TestMethod]
        public void DeleteUserNegativeIdTest() {
            Assert.IsFalse(db.DeleteUser(-1));
        }

        [TestMethod]
        public void DeleteUserNonExistingIdTest() {
            Assert.IsTrue(db.GetUser(20) == null);
            Assert.IsFalse(db.DeleteUser(20));
        }

        //Rate file
        [TestMethod]
        public void RateValidUserValidFileExistingRatingTest() {
            Assert.AreEqual(10, db.GetRating(1, 1));
            Assert.IsTrue(db.RateFile(1, 1, 5));
            Assert.AreEqual(5, db.GetRating(1, 1));

        }

        [TestMethod]
        public void RateValidUserValidFileNoExistingRatingTest() {
            Assert.AreEqual(0, db.GetNumberOfRatings(2));
            Assert.IsTrue(db.RateFile(1, 2, 4));
            Assert.AreEqual(4, db.GetRating(1, 2));
        }

        [TestMethod]
        public void RateInvalidUserValidFileTest() {
            Assert.IsFalse(db.RateFile(20, 1, 4));
        }

        [TestMethod]
        public void RateValidUserInvalidFileTest() {
            Assert.IsFalse(db.RateFile(1, 20, 4));
        }

        //Get rating
        [TestMethod]
        public void GetRatingValidUserValidFileExistingRatingTest() {
            Assert.AreEqual(10, db.GetRating(1, 1));
            Assert.IsTrue(db.RateFile(1, 1, 5));
            Assert.AreEqual(5, db.GetRating(1, 1));
        }

        [TestMethod]
        public void GetRatingValidUserValidFileNoExistingRatingTest() {
            Assert.AreEqual(0, db.GetNumberOfRatings(3));
            Assert.IsTrue(db.RateFile(1, 3, 6));
            Assert.AreEqual(6, db.GetRating(1, 3));
        }

        [TestMethod]
        public void GetRatingInvalidUserValidFileTest() {
            Assert.IsTrue(db.GetUser(20) == null);            
            Assert.AreEqual(-1, db.GetRating(20, 1));
        }

        [TestMethod]
        public void GetRatingValidUserInvalidFileTest() {
            Assert.AreEqual(-1, db.GetRating(1, 20));
        }

        //Promote to admin
        [TestMethod]
        public void PromoteValidPromoterValidPromoteeAdminTest() {
            User promoter = db.GetUser(1);
            Assert.IsTrue(promoter.IsAdmin);
            User promoteeBefore = db.GetUser(2);
            Assert.IsFalse(promoteeBefore.IsAdmin);
            db.PromotetoAdmin(1, 2);
            User promoteeAfter = db.GetUser(2);
            Assert.IsTrue(promoteeAfter.IsAdmin);
        }

        [TestMethod]
        public void PromoteValidPromoterValidPromoteeNonAdminTest() {
            User promoter = db.GetUser(4);
            Assert.IsFalse(promoter.IsAdmin);
            User promoteeBefore = db.GetUser(5);
            Assert.IsFalse(promoteeBefore.IsAdmin);
            Assert.IsFalse(db.PromotetoAdmin(4, 5));
            User promoteeAfter = db.GetUser(5);
            Assert.IsFalse(promoteeAfter.IsAdmin);
        }

        [TestMethod]
        public void PromoteInvalidPromoterValidPromoteeTest() {
            Assert.IsTrue(db.GetUser(20) == null);
            User promoteeBefore = db.GetUser(5);
            Assert.IsFalse(promoteeBefore.IsAdmin);
            Assert.IsFalse(db.PromotetoAdmin(20, 5));
            User promoteeAfter = db.GetUser(5);
            Assert.IsFalse(promoteeAfter.IsAdmin);
        }

        [TestMethod]
        public void PromoteValidPromoterInvalidPromoteeAdminTest() {
            Assert.IsTrue(db.GetUser(20) == null);
            User promoter = db.GetUser(1);
            Assert.IsTrue(promoter.IsAdmin);
            Assert.IsFalse(db.PromotetoAdmin(1, 20));
        }

        //Demote
        [TestMethod]
        public void DemoteValidDemoterValidDemoteeAdminTest() {
            User demoter = db.GetUser(1);
            Assert.IsTrue(demoter.IsAdmin);
            User demoteeBefore = db.GetUser(6);
            Assert.IsTrue(demoteeBefore.IsAdmin);
            Assert.IsTrue(db.DemoteAdmin(1, 6));
            User demoteeAfter = db.GetUser(6);
            Assert.IsFalse(demoteeAfter.IsAdmin);
        }

        [TestMethod]
        public void DemoteValidDemoterValidDemoteeNonAdminTest() {
            User demoter = db.GetUser(4);
            Assert.IsFalse(demoter.IsAdmin);
            User demoteeBefore = db.GetUser(1);
            Assert.IsTrue(demoteeBefore.IsAdmin);
            Assert.IsFalse(db.DemoteAdmin(4, 1));
            User demoteeAfter = db.GetUser(1);
            Assert.IsTrue(demoteeAfter.IsAdmin);
        }

        [TestMethod]
        public void DemoteInvalidDemoterValidDemoteeTest() {
            Assert.IsTrue(db.GetUser(20) == null);
            User demoteeBefore = db.GetUser(1);
            Assert.IsTrue(demoteeBefore.IsAdmin);
            Assert.IsFalse(db.DemoteAdmin(20, 1));
            User demoteeAfter = db.GetUser(1);
            Assert.IsTrue(demoteeAfter.IsAdmin);
        }

        [TestMethod]
        public void DemoteValidDemoterInvalidDemoteeAdminTest() {
            Assert.IsTrue(db.GetUser(20) == null);
            User demoter = db.GetUser(1);
            Assert.IsTrue(demoter.IsAdmin);
            Assert.IsFalse(db.DemoteAdmin(1, 20));
        }

        //Deposit
        [TestMethod]
        public void DepositPositiveAmountValidIdTest() {
            User userBefore = db.GetUser(4);
            Assert.AreEqual(0, userBefore.Balance);
            Assert.IsTrue(db.Deposit(20, 4));
            User userAfter = db.GetUser(4);
            Assert.AreEqual(20, userAfter.Balance);
        }

        [TestMethod]
        public void DepositNegativeAmountValidIdTest() {
            Assert.IsTrue(db.Deposit(20, 5)); //Start at 20
            User userBefore = db.GetUser(5);
            Assert.AreEqual(20, userBefore.Balance);
            Assert.IsTrue(db.Deposit(-5, 5));
            User userAfter = db.GetUser(5);
            Assert.AreEqual(15, userAfter.Balance);
        }

        [TestMethod]
        public void DepositZeroAmountValidIdTest() {
            Assert.IsTrue(db.Deposit(20, 1)); //Start at 20
            User userBefore = db.GetUser(1);
            Assert.AreEqual(20, userBefore.Balance);
            Assert.IsTrue(db.Deposit(0, 1));
            User userAfter = db.GetUser(1);
            Assert.AreEqual(20, userAfter.Balance);
        }

        [TestMethod]
        public void DepositInvalidIdTest() {
            Assert.IsTrue(db.GetUser(20) == null);
            Assert.IsFalse(db.Deposit(2, 20));

        }

        //Login
        [TestMethod]
        public void LoginValidUsernameValidPasswordTest() {
            User user = db.GetUser(2);
            Assert.AreEqual(2, db.Login(user.Username, user.Password)); 
        }

        [TestMethod]
        public void LoginInvalidUsernamePasswordTest() {
            User user1 = db.GetUser(1);
            User user2 = db.GetUser(2);
            Assert.AreEqual(-1, db.Login(user1.Username, user2.Password)); 
        }

        [TestMethod]
        public void LoginNonExistingUsernamePasswordTest() {
            User user = db.GetUser(2);
            Assert.AreEqual(-1, db.Login("Noone has this username", user.Password)); 
        }

        //AddUser
        [TestMethod]
        public void AddValidUserTest() {
            string username = "SQLrillex";
            string name="John";
            string email="Johnsmail@itu.dk";
            string password="John123";
            int balance = 0;
            User userToAdd = new User() {
                Username=username,
                Name=name,
                Email=email,
                Password=password,
                Balance=balance
            };
            User addedUser = db.AddUser(userToAdd);
            Assert.IsTrue(addedUser != null);
            //Make sure the user is initialized with the right values
            Assert.AreEqual(false, addedUser.IsAdmin);
            Assert.AreEqual(0, addedUser.Songs.Count);
            Assert.AreEqual(0, addedUser.Movies.Count);
            //compare the old and new user
            userToAdd.Id = addedUser.Id; //set the id, since this is not lazy loaded.
            _AssertUserEquality(userToAdd, addedUser);
        }

        [TestMethod]
        public void AddInvalidUserTest() {
            string username = null;
            string name = "John";
            string email = "Johnsmail@itu.dk";
            string password = "John123";
            int balance = 0;
            User userToAdd = new User() {
                Username = username,
                Name = name,
                Email = email,
                Password = password,
                Balance = balance
            };
            User addedUser = db.AddUser(userToAdd);
            Assert.AreEqual(-1, addedUser.Id); //id = -1 means the user is invalid
            Assert.IsTrue(addedUser.Name == null); //the users fields are not even set
        }

        //GetUser
        [TestMethod]
        public void GetValidUserTest() {
            User actualUser = db.GetUser(1);
            Assert.AreEqual(1, actualUser.Id);
            //Match up against known data
            Assert.IsTrue(actualUser.Name.Equals("John Doe") || actualUser.Name.Equals("Phillip De Romanero"));
            Assert.AreEqual("SmallSon", actualUser.Username);
            Assert.AreEqual("password", actualUser.Password);
            Assert.AreEqual("john@itu.dk", actualUser.Email);
            Assert.AreEqual(0, actualUser.Balance);
            //Lazy loading tested seperately
        }

        [TestMethod]
        public void GetInvalidUserTest() {
            User actualUser = db.GetUser(9000);
            Assert.IsTrue(actualUser == null);
        }

        //GetIsAdmin
        [TestMethod]
        public void GetIsAdminAdminUserTest() {
            Assert.IsTrue(db.GetIsAdmin(1));
        }

        [TestMethod]
        public void GetIsAdminNonAdminUserTest() {
            Assert.IsFalse(db.GetIsAdmin(5));
        }

        [TestMethod]
        public void GetIsAdminNonExistingUserUserTest() {
            Assert.IsFalse(db.GetIsAdmin(9000));
        }

        //GetPurchases
        [TestMethod]
        public void GetPurchasesValidUserWithPurchasesTest() {
            var purchases = db.GetPurchases(1);
            Assert.AreEqual(4, purchases.Count);
            Assert.AreEqual("Life of a Small Son", purchases[0].File.Title);
            Assert.AreEqual(new DateTime(9999, 12, 31, 23, 59, 59), purchases[0].EndTime);
            Assert.AreEqual("Life of a Small Son Title Track (Small Son)", purchases[1].File.Title);
            Assert.AreEqual(new DateTime(9999, 12, 31, 23, 59, 59), purchases[1].EndTime);
            Assert.AreEqual("A", purchases[2].File.Title);
            Assert.AreEqual(new DateTime(2012, 12, 24), purchases[2].EndTime);
            Assert.AreEqual("Sang", purchases[3].File.Title);
            Assert.AreEqual(new DateTime(2012, 12, 24), purchases[3].EndTime);
        }

        [TestMethod]
        public void GetPurchasesValidUserWithoutPurchaseTest() {
            var purchases = db.GetPurchases(2);
            Assert.AreEqual(0, purchases.Count);
        }

        [TestMethod]
        public void GetPurchasesInvalidUserTest() {
            var purchases = db.GetPurchases(9000);
            Assert.AreEqual(0, purchases.Count);
        }

        //GetSongs
        [TestMethod]
        public void GetSongsValidUserWithSongsTest() {
            var songs = db.GetSongs(1);
            Assert.AreEqual(2, songs.Count);
            Assert.AreEqual("Life of a Small Son Title Track (Small Son)", songs[0].File.Title);
            Assert.AreEqual(new DateTime(9999, 12, 31, 23, 59, 59), songs[0].EndTime);
            Assert.AreEqual("Sang", songs[1].File.Title);
            Assert.AreEqual(new DateTime(2012, 12, 24), songs[1].EndTime);
        }

        [TestMethod]
        public void GetSongsValidUserWithoutSongTest() {
            var songs = db.GetSongs(2);
            Assert.AreEqual(0, songs.Count);
        }

        [TestMethod]
        public void GetSongsInvalidUserTest() {
            var songs = db.GetSongs(9000);
            Assert.AreEqual(0, songs.Count);
        }

        //GetMovies
        [TestMethod]
        public void GetMoviesValidUserWithMoviesTest() {
            var movies = db.GetMovies(1);
            Assert.AreEqual(2, movies.Count);
            Assert.AreEqual("Life of a Small Son", movies[0].File.Title);
            Assert.AreEqual(new DateTime(9999, 12, 31, 23, 59, 59), movies[0].EndTime);
            Assert.AreEqual("A", movies[1].File.Title);
            Assert.AreEqual(new DateTime(2012, 12, 24), movies[1].EndTime);
        }

        [TestMethod]
        public void GetMoviesValidUserWithoutMovieTest() {
            var movies = db.GetMovies(2);
            Assert.AreEqual(0, movies.Count);
        }

        [TestMethod]
        public void GetMoviesInvalidUserTest() {
            var movies = db.GetMovies(9000);
            Assert.AreEqual(0, movies.Count);
        }

    }
}
