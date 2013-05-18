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

        //Update User tests
        [TestMethod]
        public void UpdateUserSameValuesTest() {

        }

        [TestMethod]
        public void UpdateUserChangedNameTest() {

        }

        [TestMethod]
        public void UpdateUserChangedUsernameTest() {

        }

        [TestMethod]
        public void UpdateUserInvalidIdTest() {

        }

        [TestMethod]
        public void UpdateUserNonExistingTest() {

        }

        //Delete User
        [TestMethod]
        public void DeleteUserValidIdTest() {

        }

        [TestMethod]
        public void DeleteUserNegativeIdTest() {

        }

        [TestMethod]
        public void DeleteUserNonExistingIdTest() {

        }

        //Rate file
        [TestMethod]
        public void RateValidUserValidFileExistingRatingTest() {

        }

        [TestMethod]
        public void RateValidUserValidFileNoExistingRatingTest() {

        }

        [TestMethod]
        public void RateInvalidUserValidFileTest() {

        }

        [TestMethod]
        public void RateValidUserInvalidFileTest() {

        }

        //Get rating
        [TestMethod]
        public void GetRatingValidUserValidFileExistingRatingTest() {

        }

        [TestMethod]
        public void GetRatingValidUserValidFileNoExistingRatingTest() {

        }

        [TestMethod]
        public void GetRatingInvalidUserValidFileTest() {

        }

        [TestMethod]
        public void GetRatingValidUserInvalidFileTest() {

        }

        //Promote to admin
        [TestMethod]
        public void PromoteValidPromoterValidPromoteeAdminTest() {

        }

        [TestMethod]
        public void PromoteValidPromoterValidPromoteeNonAdminTest() {

        }

        [TestMethod]
        public void PromoteInvalidPromoterValidPromoteeTest() {

        }

        [TestMethod]
        public void PromoteValidPromoterInvalidPromoteeAdminTest() {

        }
        //Consider one extra with invalid promotee, non admin promoter

        //Demote
        [TestMethod]
        public void DemoteValidDemoterValidDemoteeAdminTest() {

        }

        [TestMethod]
        public void DemoteValidDemoterValidDemoteeNonAdminTest() {

        }

        [TestMethod]
        public void DemoteInvalidDemoterValidDemoteeTest() {

        }

        [TestMethod]
        public void DemoteValidDemoterInvalidDemoteeAdminTest() {

        }
        //Consider one extra with invalid demotee, non admin demoter

        //Deposit
        [TestMethod]
        public void DepositPositiveAmountValidIdTest() {

        }

        [TestMethod]
        public void DepositNegativeAmountValidIdTest() {

        }

        [TestMethod]
        public void DepositZeroAmountValidIdTest() {

        }

        [TestMethod]
        public void DepositInvalidIdTest() {

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

        //GetPurchases
        [TestMethod]
        public void GetPurchasesValidUserWithPurchasesTest() {

        }

        [TestMethod]
        public void GetPurchasesValidUserWithoutPurchaseTest() {

        }

        [TestMethod]
        public void GetPurchasesInvalidUserTest() {

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
