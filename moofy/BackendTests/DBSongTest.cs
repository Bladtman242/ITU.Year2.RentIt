using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace moofy.Backend.Tests
{
    [TestClass]
    public class DBSongTest
    {
        static DBAccess db;

        public DBSongTest() {
            db = new DBAccess();
            db.Open();
            db.ClearDatabase();
            
        }

        [ClassCleanup()]
        public static void CleanUp()
        {
            db.ClearDatabase();
            db.Close();
        }

        [TestMethod]
        public void UpdateMovieValidAdminIdValidMovieTest()
        {
            Song song = db.GetSong(2);
            song.Title = "The roof is on fire";
            bool success = db.UpdateSong(song, 1);
            Song actual = db.GetSong(2);
            Assert.IsTrue(success);
            Assert.AreEqual(actual.Id, song.Id);
            Assert.AreEqual(actual.Year, song.Year);
            Assert.AreEqual(actual.Title, song.Title);
            Assert.AreEqual(actual.BuyPrice, song.BuyPrice);
            Assert.AreEqual(actual.RentPrice, song.RentPrice);
            Assert.AreEqual(actual.Artists[0], song.Artists[0]);
            Assert.AreEqual(actual.Album, song.Album);
            Assert.AreEqual(actual.Description, song.Description);
            Assert.AreEqual(actual.ViewCount, song.ViewCount);
            Assert.AreEqual(actual.Uri, song.Uri);
            Assert.AreEqual(actual.CoverUri, song.CoverUri);
        }

        [TestMethod]
        public void UpdateSongInvalidAdminIdValidSongTest()
        {
            Song song = db.GetSong(2);
            song.Title = "The floor is on fire";
            bool success = db.UpdateSong(song, -32);
            Song actual = db.GetSong(2);
            Assert.IsFalse(success);
            Assert.AreEqual(actual.Id, song.Id);
            Assert.AreEqual(actual.Year, song.Year);
            Assert.AreNotEqual(actual.Title, song.Title);
            Assert.AreEqual(actual.BuyPrice, song.BuyPrice);
            Assert.AreEqual(actual.RentPrice, song.RentPrice); 
            Assert.AreEqual(actual.Artists[0], song.Artists[0]);
            Assert.AreEqual(actual.Album, song.Album);
            Assert.AreEqual(actual.Description, song.Description);
            Assert.AreEqual(actual.ViewCount, song.ViewCount);
            Assert.AreEqual(actual.Uri, song.Uri);
            Assert.AreEqual(actual.CoverUri, song.CoverUri);
        }

        [TestMethod]
        public void UpdateSongNonAdminIdValidSongTest()
        {
            Song song = db.GetSong(2);
            song.Title = "Something is on fire";
            bool success = db.UpdateSong(song, 2);
            Song actual = db.GetSong(2);
            Assert.IsFalse(success);
            Assert.AreEqual(actual.Id, song.Id);
            Assert.AreEqual(actual.Year, song.Year);
            Assert.AreNotEqual(actual.Title, song.Title);
            Assert.AreEqual(actual.BuyPrice, song.BuyPrice);
            Assert.AreEqual(actual.RentPrice, song.RentPrice);
            Assert.AreEqual(actual.Artists[0], song.Artists[0]);
            Assert.AreEqual(actual.Album, song.Album);
            Assert.AreEqual(actual.Description, song.Description);
            Assert.AreEqual(actual.ViewCount, song.ViewCount);
            Assert.AreEqual(actual.Uri, song.Uri);
            Assert.AreEqual(actual.CoverUri, song.CoverUri);
        }
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateSongValidAdminIdInvalidMovieTest()
        {
            Song song = db.GetSong(2);
            song.Title = null;
            bool success = db.UpdateSong(song, 1);
        }

        [TestMethod]
        public void GetSongValidSongIdTest()
        {
            Song song = db.GetSong(4);
            Assert.AreEqual(song.Title, "Sang");
        }

        [TestMethod]
        public void GetSongInvalidSongIdTest()
        {
            Song song = db.GetSong(-5);
            Assert.IsNull(song);
        }

        [TestMethod]
        public void GetSongNonexistingSongIdTest()
        {
            Song song = db.GetSong(9236);
            Assert.IsNull(song);
        }

        [TestMethod]
        public void PurchaseSongValidSongIdSufficientFundsTest()
        {
            bool success = db.PurchaseSong(2, 2);
            Assert.IsTrue(success);
            IList<Purchase> songs = db.GetSongs(2);
            Assert.AreEqual(songs.Count, 1);
            Assert.AreEqual(songs[0].File.Id, 2);
        }

        [TestMethod]
        public void PurchaseSongValidSongIdInsufficientFundsTest()
        {
            bool success = db.PurchaseSong(2, 1);
            Assert.IsFalse(success);
            IList<Purchase> songs = db.GetSongs(1);
            Assert.AreEqual(songs.Count, 0);
        }

        [TestMethod]
        public void PurchaseSongInvalidUserId()
        {
            bool success = db.PurchaseSong(2, -5);
            Assert.IsFalse(success);
        }

        [TestMethod]
        public void PurchaseSongNonexistingUserId()
        {
            bool success = db.PurchaseSong(2, 55);
            Assert.IsFalse(success);
        }

        [TestMethod]
        public void RentSongValidSongIdSufficientFundsTest()
        {
            db.ClearDatabase();
            bool success = db.RentSong(2, 2, 1);
            Assert.IsTrue(success);
            IList<Purchase> songs = db.GetSongs(2);
            Assert.AreEqual(songs.Count, 1);
            Assert.AreEqual(songs[0].File.Id, 2);
        }

        [TestMethod]
        public void RentSongValidSongIdInsufficientFundsTest()
        {
            db.ClearDatabase();
            bool success = db.RentSong(2, 1, 5);
            Assert.IsFalse(success);
            IList<Purchase> songs = db.GetSongs(1);
            Assert.AreEqual(songs.Count, 0);
        }

        [TestMethod]
        public void RentSongInvalidUserIdTest()
        {
            db.ClearDatabase();
            bool success = db.RentSong(2, -5, 4);
            Assert.IsFalse(success);
        }

        [TestMethod]
        public void RentSongNonexistingUserIdTest()
        {
            db.ClearDatabase();
            bool success = db.RentSong(2, 55, 2);
            Assert.IsFalse(success);
        }

        [TestMethod]
        public void DeleteSongValidAdminIdValidSongIdTest()
        {
            bool success = db.DeleteSong(2, 1);
            Assert.IsTrue(success);
            Song song = db.GetSong(2);
            Assert.IsNull(song);
            db.ClearDatabase();
        }

        [TestMethod]
        public void DeleteSongIsNotAdminValidSongIdTest()
        {
            bool success = db.DeleteSong(2, 2);
            Assert.IsFalse(success);
            Song song = db.GetSong(2);
            Assert.IsNotNull(song);
            db.ClearDatabase();
        }

        [TestMethod]
        public void DeleteSongInvalidAdminIdValidSongIdTest()
        {
            bool success = db.DeleteSong(2, -2);
            Assert.IsFalse(success);
            Song song = db.GetSong(2);
            Assert.IsNotNull(song);
            db.ClearDatabase();
        }

        [TestMethod]
        public void DeleteSongValidAdminIdInvalidSongIdTest()
        {
            bool success = db.DeleteSong(-53, 1);
            Assert.IsFalse(success);
            db.ClearDatabase();
        }

        [TestMethod]
        public void CreateSongValidAdminIdValidTmpIdValidSongTest()
        {
            Song song = new Song()
            {
                Title = "test",
                Year = 1900,
                BuyPrice = 1000,
                RentPrice = 100,
                Description = "description",
                CoverUri = "is",
                Album = "testalbum"
            };

            Song son = db.CreateSong(1, 1, new List<String>(), song, new List<string> { "testest" });

            Song actual = db.GetSong(son.Id);

            Assert.AreEqual(actual.Id, song.Id);
            Assert.AreEqual(actual.Year, song.Year);
            Assert.AreEqual(actual.Title, song.Title);
            Assert.AreEqual(actual.BuyPrice, song.BuyPrice);
            Assert.AreEqual(actual.RentPrice, song.RentPrice);
            Assert.AreEqual(actual.Artists[0], song.Artists[0]);
            Assert.AreEqual(actual.Album, song.Album);
            Assert.AreEqual(actual.Description, song.Description);
            Assert.AreEqual(actual.ViewCount, song.ViewCount);
            Assert.AreEqual(actual.Uri, song.Uri);
            Assert.AreEqual(actual.CoverUri, song.CoverUri);
            db.ClearDatabase();
        }

        [TestMethod]
        public void CreateSongIsNotAdminTest()
        {
            Song song = new Song()
            {
                Title = "test",
                Year = 1900,
                BuyPrice = 1000,
                RentPrice = 100,
                Description = "description",
                CoverUri = "is",
                Album = "testalbum"
            };

            Song son = db.CreateSong(2, 1, new List<String>(), song, new List<string> { "testest" });
            Assert.IsNull(son);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CreateSongInvalidSongTest()
        {

            Song song = new Song()
            {
                Year = 1900,
                BuyPrice = 1000,
                RentPrice = 100,
                Description = "description"
            };
            db.CreateSong(1, 1, new List<String>(), song, new List<string> { "testest" });

        }

        [TestMethod]
        public void CreateSongInvalidAdminIdTest()
        {
            Song song = new Song()
            {
                Title = "test",
                Year = 1900,
                BuyPrice = 1000,
                RentPrice = 100,
                Description = "description",
                CoverUri = "is",
                Album = "testalbum"
            };

            Song son = db.CreateSong(-2, 1, new List<String>(), song, new List<string> { "testest" });
            Assert.IsNull(son);
        }

        [TestMethod]
        public void CreateSongInvalidTmpIdTest()
        {
            Song song = new Song()
            {
                Title = "test",
                Year = 1900,
                BuyPrice = 1000,
                RentPrice = 100,
                Description = "description",
                CoverUri = "is",
                Album = "testalbum"
            };

            Song son = db.CreateSong(1, -1, new List<String>(), song, new List<string> { "testest" });
            Assert.IsNull(son);
        }

        [TestMethod]
        public void FilterSongsValidFilterWithMatchesTest()
        {
            Song[] songs = db.FilterSongs("Vanilla");
            Assert.AreEqual(songs.Length, 1);
            Assert.AreEqual(songs[0].Id, 2);
        }

        [TestMethod]
        public void FilterSongsValidFilterWithoutMatchesTest()
        {
            Song[] songs = db.FilterSongs("big son");
            Assert.AreEqual(songs.Length, 0);
        }

        [TestMethod]
        public void FilterSongsEmptyStringTest()
        {
            Song[] songs = db.FilterSongs("");
            Assert.AreEqual(songs.Length, 2);
        }

        [TestMethod]
        public void FilterSongsNullFilterTest()
        {
            Song[] songs = db.FilterSongs(null);
            Assert.AreEqual(songs.Length, 2);
        }
        
    }
}
