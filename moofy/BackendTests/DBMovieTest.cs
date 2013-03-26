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
        public void RentMovieTest()
        {
            //Upload a file
            MemoryStream s = new MemoryStream();
            s.WriteByte(5);
            int tmpId = db.UploadFile(s);

            //Create a movie to buy with price 100
            int rent = 100;
            int movieId = db.CreateMovie(1, tmpId, "test", 1900, 1000, rent, "testest", new string[] { "Horror" }, "description");

            //Create a user to buy the movie, who can afford it
            int userId = db.AddUser(new User(0)
            {
                Name = "TestUser",
                Username = "TestUser",
                Balance = rent,
                Email = "testuser@buy.com",
                Password = "test"
            });

            Assert.AreEqual(true, db.RentMovie(movieId, userId,3));

            //Get the user from the database (required to be able to load the newly added file to his owned files)
            User actualUser = db.GetUser(userId);
            //Assert that the movie is added to movies owned by the user
            Assert.IsTrue(actualUser.Purchases.ElementAt(0).File.Id == movieId);
            TimeSpan ts = DateTime.Now.AddDays(3) - actualUser.Purchases.ElementAt(0).EndTime;
            //Assert that the expiration time is within 3days and 10minutes, the 10minutes are added as a buffer for the time it takes to run the code inbetween the RentMovie call and the creation of ts.
            //A 10minute discrepency can be accepted in the system
            Assert.IsTrue(ts.TotalMinutes < 1);

            db.DeleteUser(userId);
            db.DeleteMovie(movieId, 1);


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
            int movieId = db.CreateMovie(1, tmpId, "test", 1900, buy, 10, "testest", new string[] { "Horror" }, "description");

            //Create a user to buy the movie, who can afford it
            int userId = db.AddUser(new User(0){
                    Name = "TestUser",
                    Username = "TestUser",
                    Balance = buy,
                    Email = "testuser@buy.com",
                    Password = "test"
            });

            Assert.AreEqual(true, db.PurchaseMovie(movieId, userId));
            //Get the user from the database (required to be able to load the newly added file to his owned files)
            User actualUser = db.GetUser(userId);
            //Assert that the movie is added to movies owned by the user
            Assert.IsTrue(actualUser.Purchases.ElementAt(0).File.Id == movieId);
            //Assert that the date of expiration is set to max, comparing strings as DateTimes compare is an reference equality
            Assert.AreEqual(actualUser.Purchases.ElementAt(0).EndTime.ToString(), DateTime.MaxValue.ToString());

            db.DeleteUser(userId);
            db.DeleteMovie(movieId, 1);
            

        }
       

        [TestMethod]
        public void CreateAndGetMovieTest()
        {
            
            MemoryStream s = new MemoryStream();
            s.WriteByte(5);
            int tmpId = db.UploadFile(s);

            //Values that will be given to the CreateMovie method
            string title = "test";
            short year = 2000;
            int buy = 10;
            int rent = 1;
            string director = "test director";
            string[] genres = new string[] { "Horror" };
            string description = "test movie description";

            int id = db.CreateMovie(1, tmpId, title, year, buy, rent, director, genres, description);
            
            //The actual values in the database
            Movie actual = db.GetMovie(id);

            Assert.AreEqual(actual.Id, id);
            Assert.AreEqual(actual.Year, year);
            Assert.AreEqual(actual.Title, title);
            Assert.AreEqual(actual.BuyPrice, buy);
            Assert.AreEqual(actual.RentPrice, rent);
            Assert.AreEqual(actual.Director, director);
            Assert.AreEqual(actual.Description, description);

            db.DeleteMovie(id, 1);
        }

        [TestMethod]
        public void FilterMovieTest()
        {
            MemoryStream s = new MemoryStream();
            s.WriteByte(5);
            int tmpId1 = db.UploadFile(s);
            int tmpId2 = db.UploadFile(s);

            string title1 = "test1";
            short year1 = 2000;
            int buy1 = 10;
            int rent1 = 1;
            string director1 = "test director";
            string[] genres1 = new string[] { "Horror" };
            string description1 = "test movie description";

            int id1 = db.CreateMovie(1, tmpId1, title1, year1, buy1, rent1, director1, genres1, description1);

            string title2 = "test director";
            short year2 = 2040;
            int buy2 = 1022;
            int rent2 = 100;
            string director2 = "test idid";
            string[] genres2 = new string[] { "Romance" };
            string description2 = "test movie description22";

            int id2 = db.CreateMovie(1, tmpId2, title2, year2, buy2, rent2, director2, genres2, description2);
            
            Movie[] movies = db.FilterMovies("test director");
            
            Assert.AreEqual(movies.Length, 2);

            Movie mov1 = movies[0];
            Movie mov2 = movies[1];

            //Ensure that the first movie is equal to the first movie returned by the filter method (the movies are not returned in a specific order, but the first movie will always be returned first, insider knowledge)
            Assert.AreEqual(id1, mov1.Id);
            Assert.AreEqual(title1, mov1.Title);
            Assert.AreEqual(year1, mov1.Year);
            Assert.AreEqual(buy1, mov1.BuyPrice);
            Assert.AreEqual(rent1, mov1.RentPrice);
            Assert.AreEqual(director1, mov1.Director);
            Assert.AreEqual(description1, mov1.Description);

            //Ensure that the second movie is equal to the second movie returned by the filter method
            Assert.AreEqual(id2, mov2.Id);
            Assert.AreEqual(title2, mov2.Title);
            Assert.AreEqual(year2, mov2.Year);
            Assert.AreEqual(buy2, mov2.BuyPrice);
            Assert.AreEqual(rent2, mov2.RentPrice);
            Assert.AreEqual(director2, mov2.Director);
            Assert.AreEqual(description2, mov2.Description);

            db.DeleteMovie(id1, 1);
            db.DeleteMovie(id2, 1);
        }
    }
}
