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
            int firstMovieId = db.CreateMovie(1, tmpId, "testmovie1", 1950, 100, 10, "testdirector", new string[] { "Horror" }, "description1");
            Movie firstMovie = db.GetMovie(firstMovieId);

            //Upload another movie
            s = new MemoryStream();
            s.WriteByte(5);
            tmpId = db.UploadFile(s);
            //Create another movie to buy with price 100
            int secondMovieId = db.CreateMovie(1, tmpId, "testmovie2", 1980, 200, 30, "testdirector2", new string[] { "Action" }, "description2");
            Movie secondMovie = db.GetMovie(secondMovieId);

            //Create a user to purchase the movies
            int userId = db.AddUser(new User() {
                Name = "TestUser",
                Username = "TestUser",
                Balance = 300,
                Email = "testuser@test.com",
                Password = "test"
            });

            //Purchase the movies
            db.PurchaseMovie(firstMovieId, userId);
            db.PurchaseMovie(secondMovieId, userId);

            //Get the user movies from the database
            IList<Purchase> actualUserMovies = db.GetUser(userId).Movies;

            Assert.IsTrue(actualUserMovies.Count == 2);
            
            //check which order they are in
            Movie actualFirstMovie = null;
            Movie actualSecondMovie = null;
            if (actualUserMovies.ElementAt(0).File.Id == firstMovieId) {
                actualFirstMovie = (Movie)actualUserMovies.ElementAt(0).File;
                actualSecondMovie = (Movie)actualUserMovies.ElementAt(1).File;
            } else if (actualUserMovies.ElementAt(1).File.Id == firstMovieId) {
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
            db.DeleteUser(userId);
            db.DeleteMovie(firstMovieId, 1);
            db.DeleteMovie(secondMovieId, 1);
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
            int firstSongId = db.CreateSong(1, tmpId, "testsong1", 1950, 100, 10, "testalbum1", "testartist1", new string[] { "Horror" }, "description1");
            Song firstSong = db.GetSong(firstSongId);

            //Upload another song
            s = new MemoryStream();
            s.WriteByte(5);
            tmpId = db.UploadFile(s);
            //Create another song to buy with price 100
            int secondSongId = db.CreateSong(1, tmpId, "testsong2", 1980, 200, 30, "testalbum2", "testartist2", new string[] { "Action" }, "description2");
            Song secondSong = db.GetSong(secondSongId);

            //Create a user to purchase the songs
            int userId = db.AddUser(new User() {
                Name = "TestUser",
                Username = "TestUser",
                Balance = 300,
                Email = "testuser@test.com",
                Password = "test"
            });

            //Purchase the songs
            db.PurchaseSong(firstSongId, userId);
            db.PurchaseSong(secondSongId, userId);

            //Get the user songs from the database
            IList<Purchase> actualUserSongs = db.GetUser(userId).Songs;

            Assert.IsTrue(actualUserSongs.Count == 2);

            //check which order they are in
            Song actualFirstSong = null;
            Song actualSecondSong = null;
            if (actualUserSongs.ElementAt(0).File.Id == firstSongId) {
                actualFirstSong = (Song)actualUserSongs.ElementAt(0).File;
                actualSecondSong = (Song)actualUserSongs.ElementAt(1).File;
            } else if (actualUserSongs.ElementAt(1).File.Id == firstSongId) {
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
            db.DeleteUser(userId);
            db.DeleteSong(firstSongId, 1);
            db.DeleteSong(secondSongId, 1);
        }
    }
}
