using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace moofy.Backend.Tests
{
    /// <summary>
    /// Summary description for DBMovieTest
    /// </summary>
    [TestClass]
    public class DBMovieTest
    {
        static DBAccess db;
        public DBMovieTest() {
            db = new DBAccess();
            db.Open();
            db.ClearDatabase();
            
        }

        [ClassCleanup()]
        public static void CleanUp() {
            db.ClearDatabase();
            db.Close();
        }

        [TestMethod]
        public void UpdateMovieValidAdminIdValidMovieTest()
        {
            Movie movie = db.GetMovie(1);
            movie.Title = "En russer rydder op i Chicago og New York";
            bool success = db.UpdateMovie(movie, 1);
            Movie actual = db.GetMovie(1);
            Assert.IsTrue(success);
            Assert.AreEqual(actual.Id, movie.Id);
            Assert.AreEqual(actual.Year, movie.Year);
            Assert.AreEqual(actual.Title, movie.Title);
            Assert.AreEqual(actual.BuyPrice, movie.BuyPrice);
            Assert.AreEqual(actual.RentPrice, movie.RentPrice);
            Assert.AreEqual(actual.Directors[0], movie.Directors[0]);
            Assert.AreEqual(actual.Description, movie.Description);
            Assert.AreEqual(actual.ViewCount, movie.ViewCount);
            Assert.AreEqual(actual.Uri, movie.Uri);
            Assert.AreEqual(actual.CoverUri, movie.CoverUri);
        }

        [TestMethod]
        public void UpdateMovieInvalidAdminIdValidMovieTest()
        {
            Movie movie = db.GetMovie(1);
            movie.Title = "En russer rydder op i Chicago og New York og Køge";
            bool success = db.UpdateMovie(movie, -32);
            Movie actual = db.GetMovie(1);
            Assert.IsFalse(success);
            Assert.AreEqual(actual.Id, movie.Id);
            Assert.AreEqual(actual.Year, movie.Year);
            Assert.AreNotEqual(actual.Title, movie.Title);
            Assert.AreEqual(actual.BuyPrice, movie.BuyPrice);
            Assert.AreEqual(actual.RentPrice, movie.RentPrice);
            Assert.AreEqual(actual.Directors[0], movie.Directors[0]);
            Assert.AreEqual(actual.Description, movie.Description);
            Assert.AreEqual(actual.ViewCount, movie.ViewCount);
            Assert.AreEqual(actual.Uri, movie.Uri);
            Assert.AreEqual(actual.CoverUri, movie.CoverUri);
        }

        [TestMethod]
        public void UpdateMovieNonAdminIdValidMovieTest()
        {
            Movie movie = db.GetMovie(1);
            movie.Title = "En russer rydder op i Chicago og New York og Køge og Jerusalem";
            bool success = db.UpdateMovie(movie, 2);
            Movie actual = db.GetMovie(1);
            Assert.IsFalse(success);
            Assert.AreEqual(actual.Id, movie.Id);
            Assert.AreEqual(actual.Year, movie.Year);
            Assert.AreNotEqual(actual.Title, movie.Title);
            Assert.AreEqual(actual.BuyPrice, movie.BuyPrice);
            Assert.AreEqual(actual.RentPrice, movie.RentPrice);
            Assert.AreEqual(actual.Directors[0], movie.Directors[0]);
            Assert.AreEqual(actual.Description, movie.Description);
            Assert.AreEqual(actual.ViewCount, movie.ViewCount);
            Assert.AreEqual(actual.Uri, movie.Uri);
            Assert.AreEqual(actual.CoverUri, movie.CoverUri);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateMovieValidAdminIdInvalidMovieTest()
        {
            Movie movie = db.GetMovie(1);
            movie.Title = null;
            bool success = db.UpdateMovie(movie, 1);
        }

        [TestMethod]
        public void GetMovieValidMovieIdTest()
        {
            Movie movie = db.GetMovie(3);
            Assert.AreEqual(movie.Title, "A");
        }

        [TestMethod]
        public void GetMovieInvalidMovieIdTest()
        {
            Movie movie = db.GetMovie(-5);
            Assert.IsNull(movie);
        }

        [TestMethod]
        public void GetMovieNonexistingMovieIdTest()
        {
            Movie movie = db.GetMovie(9236);
            Assert.IsNull(movie);
        }

        [TestMethod]
        public void PurchaseMovieValidMovieIdSufficientFundsTest()
        {
            bool success = db.PurchaseMovie(1, 2);
            Assert.IsTrue(success);
            IList<Purchase> movies = db.GetMovies(2);
            Assert.AreEqual(movies.Count, 1);
            Assert.AreEqual(movies[0].File.Id, 1);
        }

        [TestMethod]
        public void PurchaseMovieValidMovieIdInsufficientFundsTest()
        {
            bool success = db.PurchaseMovie(1, 1);
            Assert.IsFalse(success);
            IList<Purchase> movies = db.GetMovies(1);
            Assert.AreEqual(movies.Count, 0);
        }

        [TestMethod]
        public void PurchaseMovieInvalidUserId()
        {
            bool success = db.PurchaseMovie(1, -5);
            Assert.IsFalse(success);
        }

        [TestMethod]
        public void PurchaseMovieNonexistingUserId()
        {
            bool success = db.PurchaseMovie(1, 55);
            Assert.IsFalse(success);
        }

        [TestMethod]
        public void RentMovieValidMovieIdSufficientFundsTest()
        {
            db.ClearDatabase();
            bool success = db.RentMovie(1, 2, 1);
            Assert.IsTrue(success);
            IList<Purchase> movies = db.GetMovies(2);
            Assert.AreEqual(movies.Count, 1);
            Assert.AreEqual(movies[0].File.Id, 1);
        }

        [TestMethod]
        public void RentMovieValidMovieIdInsufficientFundsTest()
        {
            db.ClearDatabase();
            bool success = db.RentMovie(1, 1, 5);
            Assert.IsFalse(success);
            IList<Purchase> movies = db.GetMovies(1);
            Assert.AreEqual(movies.Count, 0);
        }

        [TestMethod]
        public void RentMovieInvalidUserIdTest()
        {
            db.ClearDatabase();
            bool success = db.RentMovie(1, -5, 4);
            Assert.IsFalse(success);
        }

        [TestMethod]
        public void RentMovieNonexistingUserIdTest()
        {
            db.ClearDatabase();
            bool success = db.RentMovie(1, 55, 2);
            Assert.IsFalse(success);
        }

        [TestMethod]
        public void DeleteMovieValidAdminIdValidMovieIdTest()
        {
            bool success = db.DeleteMovie(3, 1);
            Assert.IsTrue(success);
            Movie movie = db.GetMovie(3);
            Assert.IsNull(movie);
            db.ClearDatabase();
        }

        [TestMethod]
        public void DeleteMovieIsNotAdminValidMovieIdTest()
        {
            bool success = db.DeleteMovie(3, 2);
            Assert.IsFalse(success);
            Movie movie = db.GetMovie(3);
            Assert.IsNotNull(movie);
            db.ClearDatabase();
        }

        [TestMethod]
        public void DeleteMovieInvalidAdminIdValidMovieIdTest()
        {
            bool success = db.DeleteMovie(3, -2);
            Assert.IsFalse(success);
            Movie movie = db.GetMovie(3);
            Assert.IsNotNull(movie);
            db.ClearDatabase();
        }

        [TestMethod]
        public void DeleteMovieValidAdminIdInvalidMovieIdTest()
        {
            bool success = db.DeleteMovie(-53, 1);
            Assert.IsFalse(success);
            db.ClearDatabase();
        }

        [TestMethod]
        public void CreateMovieValidAdminIdValidTmpIdValidMovieTest()
        {
            Movie movie = new Movie()
            {
                Title = "test",
                Year = 1900,
                BuyPrice = 1000,
                RentPrice = 100,
                Description = "description",
                CoverUri = "is"
            };

            Movie mov = db.CreateMovie(1, 1, new List<String>(), movie, new List<string> { "testest" });

            Movie actual = db.GetMovie(mov.Id);

            Assert.AreEqual(actual.Id, movie.Id);
            Assert.AreEqual(actual.Year, movie.Year);
            Assert.AreEqual(actual.Title, movie.Title);
            Assert.AreEqual(actual.BuyPrice, movie.BuyPrice);
            Assert.AreEqual(actual.RentPrice, movie.RentPrice);
            Assert.AreEqual(actual.Directors[0], movie.Directors[0]);
            Assert.AreEqual(actual.Description, movie.Description);
            Assert.AreEqual(actual.ViewCount, movie.ViewCount);
            Assert.AreEqual(actual.Uri, movie.Uri);
            Assert.AreEqual(actual.CoverUri, movie.CoverUri);
            db.ClearDatabase();
        }

        [TestMethod]
        public void CreateMovieIsNotAdminTest()
        {
            Movie movie = new Movie()
            {
                Title = "test",
                Year = 1900,
                BuyPrice = 1000,
                RentPrice = 100,
                Description = "description",
                CoverUri = "is"
            };

            Movie mov = db.CreateMovie(2, 1, new List<String>(), movie, new List<string> { "testest" });
            Assert.IsNull(mov);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CreateMovieInvalidMovieTest()
        {
            
            Movie movie = new Movie()
            {
                Year = 1900,
                BuyPrice = 1000,
                RentPrice = 100,
                Description = "description"
            };
            db.CreateMovie(1, 1, new List<String>(), movie, new List<string> { "testest" });

        }

        [TestMethod]
        public void CreateMovieInvalidAdminIdTest()
        {
            Movie movie = new Movie()
            {
                Title = "test",
                Year = 1900,
                BuyPrice = 1000,
                RentPrice = 100,
                Description = "description",
                CoverUri = "is"
            };

            Movie mov = db.CreateMovie(-2, 1, new List<String>(), movie, new List<string> { "testest" });
            Assert.IsNull(mov);
        }

        [TestMethod]
        public void CreateMovieInvalidTmpIdTest()
        {
            Movie movie = new Movie()
            {
                Title = "test",
                Year = 1900,
                BuyPrice = 1000,
                RentPrice = 100,
                Description = "description",
                CoverUri = "is"
            };

            Movie mov = db.CreateMovie(1, -1, new List<String>(), movie, new List<string> { "testest" });
            Assert.IsNull(mov);
        }

        [TestMethod]
        public void FilterMoviesValidFilterWithMatchesTest()
        {
            Movie[] movies = db.FilterMovies("Small son");
            Assert.AreEqual(movies.Length, 1);
            Assert.AreEqual(movies[0].Id, 1);
        }

        [TestMethod]
        public void FilterMoviesValidFilterWithoutMatchesTest()
        {
            Movie[] movies = db.FilterMovies("big son");
            Assert.AreEqual(movies.Length, 0);
        }

        [TestMethod]
        public void FilterMoviesEmptyStringTest()
        {
            Movie[] movies = db.FilterMovies("");
            Assert.AreEqual(movies.Length, 2);
        }

        [TestMethod]
        public void FilterMoviesNullFilterTest()
        {
            Movie[] movies = db.FilterMovies(null);
            Assert.AreEqual(movies.Length, 2);
        }
        
    }
}
