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
            
        }

        [ClassCleanup()]
        public static void CleanUp() {
            db.Close();
        }
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
            bool isUpdated = db.UpdateMovie(newMov);
            Assert.IsTrue(isUpdated);
            Movie actual = db.GetMovie(newMov.Id);
            Assert.AreEqual(newMov.Title, actual.Title);
            Assert.AreEqual(newMov.Director, actual.Director);
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
        }
    }
}
