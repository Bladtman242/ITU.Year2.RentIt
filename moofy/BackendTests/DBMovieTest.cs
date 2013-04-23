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
        /*
        [TestMethod]
        public void UpdateMovieTest()
        {
            //Upload a file
            MemoryStream s = new MemoryStream();
            s.WriteByte(5);
            int tmpId = db.UploadFile(s);

            //Create a movie to edit
            int rent = 100;
            Movie mov = new Movie()
            {
                Title = "test",
                Year = 1900,
                BuyPrice = 1000,
                RentPrice = rent,
                Director = "testest",
                Description = "description"
            };
            Movie movie = db.CreateMovie(1, tmpId, new string[] { "Horror" }, mov);

            //Create u new movie with the updated information
            Movie newMov = new Movie()
            {
                Id = movie.Id,
                Title = "NEWTITLE",
                Year = 1900,
                BuyPrice = 1000,
                RentPrice = rent,
                Director = "IFIGAST",
                Description = "descriptio1111n"
            };
            bool isUpdated = db.UpdateMovie(newMov,1);
            bool cleared = db.ClearFileGenres(newMov.Id);
            bool genreGood = db.AddAllGenres(newMov.Id, new string[] { "Romance", "Funky", "Pop" });
            Assert.IsTrue(isUpdated);
            Assert.IsTrue(genreGood);
            Assert.IsTrue(cleared);
            Movie actual = db.GetMovie(newMov.Id);
            Assert.AreEqual(newMov.Title, actual.Title);
            Assert.AreEqual(newMov.Director, actual.Director);
            Assert.IsTrue(newMov.Genres.Count == 3);
            Assert.IsTrue(newMov.Genres.Contains("Romance"));
            Assert.IsTrue(newMov.Genres.Contains("Funky"));
            Assert.IsTrue(newMov.Genres.Contains("Pop"));
            //Cleanup
            db.DeleteMovie(newMov.Id, 1);
        }
        [TestMethod]
        public void RentMovieTest()
        {
            //Upload a file
            MemoryStream s = new MemoryStream();
            s.WriteByte(5);
            int tmpId = db.UploadFile(s);

            //Create a movie to buy with price 100
            int rent = 100;
            Movie mov = new Movie()
            {
                Title = "test",
                Year = 1900,
                BuyPrice = 1000,
                RentPrice = rent,
                Director = "testest",
                Description = "description"
            };
            Movie movie = db.CreateMovie(1, tmpId, new string[] { "Horror" }, mov);

            //Create a user to buy the movie, who can afford it
            User user = db.AddUser(new User()
            {
                Name = "TestUser",
                Username = "TestUser",
                Balance = rent,
                Email = "testuser@buy.com",
                Password = "test"
            });

            Assert.AreEqual(true, db.RentMovie(movie.Id, user.Id,3));

            //Get the user from the database (required to be able to load the newly added file to his owned files)
            User actualUser = db.GetUser(user.Id);
            //Assert that the movie is added to movies owned by the user
            Assert.IsTrue(actualUser.Movies.ElementAt(0).File.Id == movie.Id);
            TimeSpan ts = DateTime.Now.AddDays(3) - actualUser.Movies.ElementAt(0).EndTime;
            //Assert that the expiration time is within 3days and 10minutes, the 10minutes are added as a buffer for the time it takes to run the code inbetween the RentMovie call and the creation of ts.
            //A 10minute discrepency can be accepted in the system
            Assert.IsTrue(ts.TotalMinutes < 1);

            db.DeleteUser(user.Id);
            db.DeleteMovie(movie.Id, 1);


        }

        [TestMethod]
        public void BuyMovieTest()
        {
            //Upload a file
            MemoryStream s = new MemoryStream();
            s.WriteByte(5);
            int tmpId = db.UploadFile(s);
            
            //Create a movie to buy with price 100
            int buy = 100;
            Movie mov = new Movie()
            {
                Title = "test",
                Year = 1900,
                BuyPrice = buy,
                RentPrice = 10,
                Director = "testest",
                Description = "description"
            };
            Movie movie = db.CreateMovie(1, tmpId,new string[] { "Horror" }, mov);

            //Create a user to buy the movie, who can afford it
            User user = db.AddUser(new User(){
                    Name = "TestUser",
                    Username = "TestUser",
                    Balance = buy,
                    Email = "testuser@buy.com",
                    Password = "test"
            });

            Assert.AreEqual(true, db.PurchaseMovie(movie.Id, user.Id));
            //Get the user from the database (required to be able to load the newly added file to his owned files)
            User actualUser = db.GetUser(user.Id);
            //Assert that the movie is added to movies owned by the user
            Assert.IsTrue(actualUser.Movies.ElementAt(0).File.Id == movie.Id);
            //Assert that the date of expiration is set to max, comparing strings as DateTimes compare is an reference equality
            Assert.AreEqual(actualUser.Movies.ElementAt(0).EndTime.ToString(), DateTime.MaxValue.ToString());

            db.DeleteUser(user.Id);
            db.DeleteMovie(movie.Id, 1);
        }
       

        [TestMethod]
        public void CreateAndGetMovieTest()
        {
            
            MemoryStream s = new MemoryStream();
            s.WriteByte(5);
            int tmpId = db.UploadFile(s);

            //Values that will be given to the CreateMovie method
            Movie mov = new Movie()
            {
                Title = "test",
                Year = 2000,
                BuyPrice = 10,
                RentPrice = 1,
                Director = "test director",
                Description = "test movie description"
            };
            
            string[] genres = new string[] { "Horror" };

            Movie movie = db.CreateMovie(1, tmpId, genres, mov);
            
            //The actual values in the database
            Movie actual = db.GetMovie(movie.Id);

            Assert.AreEqual(actual.Id, movie.Id);
            Assert.AreEqual(actual.Year, movie.Year);
            Assert.AreEqual(actual.Title, movie.Title);
            Assert.AreEqual(actual.BuyPrice, movie.BuyPrice);
            Assert.AreEqual(actual.RentPrice, movie.RentPrice);
            Assert.AreEqual(actual.Director, movie.Director);
            Assert.AreEqual(actual.Description, movie.Description);

            db.DeleteMovie(movie.Id, 1);
        }

        [TestMethod]
        public void FilterMovieTest()
        {
            MemoryStream s = new MemoryStream();
            s.WriteByte(5);
            int tmpId1 = db.UploadFile(s);
            int tmpId2 = db.UploadFile(s);

            Movie mo1 = new Movie()
            {
                Title = "test1",
                Year = 2000,
                BuyPrice = 10,
                RentPrice = 1,
                Director = "test director",
                Description = "test movie description"
            };
            
            string[] genres1 = new string[] { "Horror" };
            
            Movie movie1 = db.CreateMovie(1, tmpId1, genres1, mo1);

            Movie mo2 = new Movie()
            {
                Title = "test director",
                Year = 2040,
                BuyPrice = 1022,
                RentPrice = 100,
                Director = "test idid",
                Description = "test movie description22"
            };
            
            string[] genres2 = new string[] { "Romance" };

            Movie movie2 = db.CreateMovie(1, tmpId2, genres2, mo2);
            
            Movie[] movies = db.FilterMovies("test director");
            
            Assert.AreEqual(movies.Length, 2);

            Movie mov1 = movies[0];
            Movie mov2 = movies[1];

            //Ensure that the first movie is equal to the first movie returned by the filter method (the movies are not returned in a specific order, but the first movie will always be returned first, insider knowledge)
            Assert.AreEqual(movie1.Id, mov1.Id);
            Assert.AreEqual(movie1.Title, mov1.Title);
            Assert.AreEqual(movie1.Year, mov1.Year);
            Assert.AreEqual(movie1.BuyPrice, mov1.BuyPrice);
            Assert.AreEqual(movie1.RentPrice, mov1.RentPrice);
            Assert.AreEqual(movie1.Director, mov1.Director);
            Assert.AreEqual(movie1.Description, mov1.Description);

            //Ensure that the second movie is equal to the second movie returned by the filter method
            Assert.AreEqual(movie2.Id, mov2.Id);
            Assert.AreEqual(movie2.Title, mov2.Title);
            Assert.AreEqual(movie2.Year, mov2.Year);
            Assert.AreEqual(movie2.BuyPrice, mov2.BuyPrice);
            Assert.AreEqual(movie2.RentPrice, mov2.RentPrice);
            Assert.AreEqual(movie2.Director, mov2.Director);
            Assert.AreEqual(movie2.Description, mov2.Description);

            db.DeleteMovie(movie1.Id, 1);
            db.DeleteMovie(movie2.Id, 1);
        }*/
    }
}
