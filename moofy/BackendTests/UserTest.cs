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
        /// Tests whether the movies are retrieved correctly in the user object
        /// </summary>
        [TestMethod]
        public void MoviesTest() {
            //Upload a movie
            MemoryStream s = new MemoryStream();
            s.WriteByte(5);
            int tmpId = db.UploadFile(s);
            //Create a movie
            Movie mo1 = new Movie()
            {
                Title = "testmovie1",
                Year = 1950,
                BuyPrice = 100,
                RentPrice = 10,
                Director = "testdirector",
                Description = "description1"
            };
            Movie firstMovie = db.CreateMovie(1, tmpId, new string[] { "Horror" }, mo1);

            //Upload another movie
            s = new MemoryStream();
            s.WriteByte(5);
            tmpId = db.UploadFile(s);
            //Create another movie to buy with price 100
            Movie mo2 = new Movie()
            {
                Title = "testmovie2",
                Year = 1980,
                BuyPrice = 200,
                RentPrice = 30,
                Director = "testdirector2",
                Description = "description2"
            };
            Movie secondMovie = db.CreateMovie(1, tmpId, new string[] { "Action" }, mo2);

            //Create a user to purchase the movies
            User user = db.AddUser(new User() {
                Name = "TestUser",
                Username = "TestUser",
                Balance = 300,
                Email = "testuser@test.com",
                Password = "test"
            });

            //Purchase the movies
            db.PurchaseMovie(firstMovie.Id, user.Id);
            db.PurchaseMovie(secondMovie.Id, user.Id);

            //Get the user movies from the database
            IList<Purchase> actualUserMovies = db.GetUser(user.Id).Movies;

            Assert.IsTrue(actualUserMovies.Count == 2);
            
            //check which order they are in
            Movie actualFirstMovie = null;
            Movie actualSecondMovie = null;
            if (actualUserMovies.ElementAt(0).File.Id == firstMovie.Id) {
                actualFirstMovie = (Movie)actualUserMovies.ElementAt(0).File;
                actualSecondMovie = (Movie)actualUserMovies.ElementAt(1).File;
            } else if (actualUserMovies.ElementAt(1).File.Id == firstMovie.Id) {
                actualSecondMovie = (Movie)actualUserMovies.ElementAt(0).File;
                actualFirstMovie = (Movie)actualUserMovies.ElementAt(1).File;
            } else {
                Assert.Fail("First movie was not retrieved");
            }

            //Asserts
            Assert.AreEqual(firstMovie.Title, actualFirstMovie.Title);
            Assert.AreEqual(firstMovie.Year, actualFirstMovie.Year);
            Assert.AreEqual(firstMovie.BuyPrice, actualFirstMovie.BuyPrice);
            Assert.AreEqual(firstMovie.RentPrice, actualFirstMovie.RentPrice);
            Assert.AreEqual(firstMovie.Director, actualFirstMovie.Director);
            Assert.IsTrue(firstMovie.Genres.SequenceEqual(actualFirstMovie.Genres));
            Assert.AreEqual(firstMovie.Description, actualFirstMovie.Description);

            Assert.AreEqual(secondMovie.Title, actualSecondMovie.Title);
            Assert.AreEqual(secondMovie.Year, actualSecondMovie.Year);
            Assert.AreEqual(secondMovie.BuyPrice, actualSecondMovie.BuyPrice);
            Assert.AreEqual(secondMovie.RentPrice, actualSecondMovie.RentPrice);
            Assert.AreEqual(secondMovie.Director, actualSecondMovie.Director);
            Assert.IsTrue(secondMovie.Genres.SequenceEqual(actualSecondMovie.Genres));
            Assert.AreEqual(secondMovie.Description, actualSecondMovie.Description);
            
            //clean-up
            db.DeleteUser(user.Id);
            db.DeleteMovie(firstMovie.Id, 1);
            db.DeleteMovie(secondMovie.Id, 1);
        }

        /// <summary>
        /// Tests whether the Songs are retrieved correctly in the user object
        /// </summary>
        [TestMethod]
        public void SongsTest() {
            //Upload a song
            MemoryStream s = new MemoryStream();
            s.WriteByte(5);
            int tmpId = db.UploadFile(s);
            //Create a song
            Song song1 = new Song()
            {
                Title = "testsong1",
                Year = 1950,
                BuyPrice = 100,
                RentPrice = 10,
                Album = "testalbum1",
                Artist = "testartist1",
                Description = "description1"
            };
            Song firstSong = db.CreateSong(1, tmpId,  new string[] { "Horror" }, song1);
            

            //Upload another song
            s = new MemoryStream();
            s.WriteByte(5);
            tmpId = db.UploadFile(s);
            //Create another song to buy with price 100
            Song song2 = new Song()
            {
                Title = "testsong2",
                Year = 1980,
                BuyPrice = 200,
                RentPrice = 30,
                Album = "testalbum2",
                Artist = "testartist2",
                Description = "description2"
            };
            Song secondSong = db.CreateSong(1, tmpId, new string[] { "Action" },song2);

            //Create a user to purchase the songs
            User user = db.AddUser(new User() {
                Name = "TestUser",
                Username = "TestUser",
                Balance = 300,
                Email = "testuser@test.com",
                Password = "test"
            });

            //Purchase the songs
            db.PurchaseSong(firstSong.Id, user.Id);
            db.PurchaseSong(secondSong.Id, user.Id);

            //Get the user songs from the database
            IList<Purchase> actualUserSongs = db.GetUser(user.Id).Songs;

            Assert.IsTrue(actualUserSongs.Count == 2);

            //check which order they are in
            Song actualFirstSong = null;
            Song actualSecondSong = null;
            if (actualUserSongs.ElementAt(0).File.Id == firstSong.Id) {
                actualFirstSong = (Song)actualUserSongs.ElementAt(0).File;
                actualSecondSong = (Song)actualUserSongs.ElementAt(1).File;
            } else if (actualUserSongs.ElementAt(1).File.Id == firstSong.Id) {
                actualSecondSong = (Song)actualUserSongs.ElementAt(0).File;
                actualFirstSong = (Song)actualUserSongs.ElementAt(1).File;
            } else {
                Assert.Fail("First song was not retrieved");
            }

            //Asserts
            Assert.AreEqual(firstSong.Title, actualFirstSong.Title);
            Assert.AreEqual(firstSong.Year, actualFirstSong.Year);
            Assert.AreEqual(firstSong.BuyPrice, actualFirstSong.BuyPrice);
            Assert.AreEqual(firstSong.RentPrice, actualFirstSong.RentPrice);
            Assert.AreEqual(firstSong.Album, actualFirstSong.Album);
            Assert.AreEqual(firstSong.Artist, actualFirstSong.Artist);
            Assert.IsTrue(firstSong.Genres.SequenceEqual(actualFirstSong.Genres));
            Assert.AreEqual(firstSong.Description, actualFirstSong.Description);

            Assert.AreEqual(secondSong.Title, actualSecondSong.Title);
            Assert.AreEqual(secondSong.Year, actualSecondSong.Year);
            Assert.AreEqual(secondSong.BuyPrice, actualSecondSong.BuyPrice);
            Assert.AreEqual(secondSong.RentPrice, actualSecondSong.RentPrice);
            Assert.AreEqual(secondSong.Album, actualSecondSong.Album);
            Assert.AreEqual(secondSong.Artist, actualSecondSong.Artist);
            Assert.IsTrue(secondSong.Genres.SequenceEqual(actualSecondSong.Genres));
            Assert.AreEqual(secondSong.Description, actualSecondSong.Description);

            //clean-up
            db.DeleteUser(user.Id);
            db.DeleteSong(firstSong.Id, 1);
            db.DeleteSong(secondSong.Id, 1);
        }
    }
}
