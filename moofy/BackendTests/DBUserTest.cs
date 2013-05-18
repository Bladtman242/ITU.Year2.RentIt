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
            Assert.IsTrue(actual.Movies.SequenceEqual<Purchase>(expected.Movies));
            Assert.IsTrue(actual.Songs.SequenceEqual<Purchase>(expected.Songs));
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

        }

        [TestMethod]
        public void LoginInvalidUsernamePasswordTest() {

        }

        [TestMethod]
        public void LoginNonExistingUsernamePasswordTest() {

        }

        //AddUser
        [TestMethod]
        public void AddValidUserTest() {

        }

        [TestMethod]
        public void AddInvalidUserTest() {

        }

        //GetUser
        [TestMethod]
        public void GetValidUserTest() {

        }

        [TestMethod]
        public void GetInvalidUserTest() {

        }

        //GetIsAdmin
        [TestMethod]
        public void GetIsAdminAdminUserTest() {

        }

        [TestMethod]
        public void GetIsAdminNonAdminUserTest() {

        }

        [TestMethod]
        public void GetIsAdminNonExistingUserUserTest() {

        }

        //GetSongs
        [TestMethod]
        public void GetSongsValidUserWithSongsTest() {

        }

        [TestMethod]
        public void GetSongsValidUserWithoutSongTest() {

        }

        [TestMethod]
        public void GetSongsInvalidUserTest() {

        }

        //GetMovies
        [TestMethod]
        public void GetMoviesValidUserWithMoviesTest() {

        }

        [TestMethod]
        public void GetMoviesValidUserWithoutMovieTest() {

        }

        [TestMethod]
        public void GetMoviesInvalidUserTest() {

        }

    }
}
