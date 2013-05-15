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
        /*
        [TestMethod]
        public void UpdateSongTest()
        {
            //Upload a file
            MemoryStream s = new MemoryStream();
            s.WriteByte(5);
            int tmpId = db.UploadFile(s);

            int rent = 100;
            Song song = new Song()
            {
                Title = "test",
                Year = 1900,
                BuyPrice = 1000,
                RentPrice = rent,
                Album = "testalbum",
                Description = "description"
            };
            Song song1 = db.CreateSong(1, tmpId, new string[] { "Horror" }, song, new List<string>{"testartist"});

            Song newSong = new Song()
            {
                Id = song1.Id,
                Title = "NEWTITLE",
                Year = 1909,
                BuyPrice = 1000,
                RentPrice = rent,
                Album = "NEWALBUM",
                Description = "description"
            };
            bool isUpdated = db.UpdateSong(newSong,1);
            Assert.IsTrue(isUpdated);
            Song actual = db.GetSong(newSong.Id);
            Assert.AreEqual(newSong.Title, actual.Title);
            Assert.AreEqual(newSong.Album, actual.Album);
            //Cleanup
            db.DeleteSong(newSong.Id,1);
        }
        [TestMethod]
        public void RentSongTest()
        {
            //Upload a file
            MemoryStream s = new MemoryStream();
            s.WriteByte(5);
            int tmpId = db.UploadFile(s);

            //Create a movie to buy with price 100
            int rent = 100;
            Song song = new Song(){
                Title = "test0000",
                Year = 1900,
                BuyPrice = 1000,
                RentPrice = rent,
                Album = "testalbum",
                Description = "description"
            };
            Song song1 = db.CreateSong(1, tmpId, new string[] { "Horror" }, song, new List<string> { "testartist" });

            //Create a user to buy the movie, who can afford it
            User user = db.AddUser(new User()
            {
                Name = "TestUser",
                Username = "TestUser",
                Balance = rent,
                Email = "testuser@buy.com",
                Password = "test"
            });

            Assert.IsTrue(db.RentSong(song1.Id, user.Id, 3));

            //Get the user from the database (required to be able to load the newly added file to his owned files)
            User actualUser = db.GetUser(user.Id);
            //Assert that the movie is added to movies owned by the user
            Assert.IsTrue(actualUser.Songs.ElementAt(0).File.Id == song1.Id);
            TimeSpan ts = DateTime.Now.AddDays(3) - actualUser.Songs.ElementAt(0).EndTime;
            //Assert that the expiration time is within 3days and 10minutes, the 10minutes are added as a buffer for the time it takes to run the code inbetween the RentMovie call and the creation of ts.
            //A 10minute discrepency can be accepted in the system
            Assert.IsTrue(ts.TotalMinutes < 1);

            db.DeleteUser(user.Id);
            db.DeleteSong(song.Id, 1);


        }
        
        [TestMethod]
        public void BuySongTest()
        {
            //Upload a file
            MemoryStream s = new MemoryStream();
            s.WriteByte(5);
            int tmpId = db.UploadFile(s);
            
            //Create a movie to buy with price 100
            int buy = 100;
            Song song = new Song()
            {
                Title = "test",
                Year = 1900,
                BuyPrice = buy,
                RentPrice = 10,
                Album = "testalbum",
                Description = "description"
            };
            Song actSong = db.CreateSong(1, tmpId, new string[] { "Horror" }, song, new List<string> { "testartist" });

            //Create a user to buy the movie, who can afford it
            User user = db.AddUser(new User(){
                    Name = "TestUser",
                    Username = "TestUser",
                    Balance = buy,
                    Email = "testuser@buy.com",
                    Password = "test"
            });

            Assert.IsTrue(db.PurchaseSong(actSong.Id, user.Id));
            //Get the user from the database (required to be able to load the newly added file to his owned files)
            User actualUser = db.GetUser(user.Id);
            //Assert that the movie is added to movies owned by the user
            Assert.IsTrue(actualUser.Songs.ElementAt(0).File.Id == actSong.Id);
            //Assert that the date of expiration is set to max, comparing strings as DateTimes compare is an reference equality
            Assert.AreEqual(actualUser.Songs.ElementAt(0).EndTime.ToString(), DateTime.MaxValue.ToString());

            db.DeleteUser(user.Id);
            db.DeleteSong(actSong.Id, 1);
            

        }
       
        
        [TestMethod]
        public void CreateAndGetSongTest()
        {
            
            MemoryStream s = new MemoryStream();
            s.WriteByte(5);
            int tmpId = db.UploadFile(s);

            //Values that will be given to the CreateMovie method
            Song song = new Song()
            {
                Title = "test",
                Year = 2000,
                BuyPrice = 10,
                RentPrice = 1,
                Album = "test album",
                Description = "test song description"
            };
            string[] genres = new string[] { "Horror" };

            Song expected = db.CreateSong(1, tmpId, genres, song, new List<string> { "test artist" });
            
            //The actual values in the database
            Song actual = db.GetSong(expected.Id);

            Assert.AreEqual(actual.Id, expected.Id);
            Assert.AreEqual(actual.Year, expected.Year);
            Assert.AreEqual(actual.Title, expected.Title);
            Assert.AreEqual(actual.BuyPrice, expected.BuyPrice);
            Assert.AreEqual(actual.RentPrice, expected.RentPrice);
            Assert.AreEqual(actual.Album, expected.Album);
            Assert.AreEqual(actual.Artists[0], expected.Artists[0]);
            Assert.AreEqual(actual.Description, expected.Description);

            db.DeleteSong(song.Id, 1);
        }
        
        [TestMethod]
        public void FilterSongTest()
        {
            MemoryStream s = new MemoryStream();
            s.WriteByte(5);
            int tmpId1 = db.UploadFile(s);
            int tmpId2 = db.UploadFile(s);

            Song s1 = new Song()
            {
                Title = "test1",
                Year = 2000,
                BuyPrice = 10,
                RentPrice = 1,
                Album = "test album",
                Description = "test song description"
            };
            string[] genres1 = new string[] { "Horror" };

            Song song1 = db.CreateSong(1, tmpId1, genres1, s1, new List<string> { "test artist" });

            Song s2 = new Song()
            {
                Title = "test album",
                Year = 2040,
                BuyPrice = 1022,
                RentPrice = 100,
                Album = "test albummmmmmmmm",
                Description = "test song description2"
            };
            
            string[] genres2 = new string[] { "Comedy" };

            Song song2 = db.CreateSong(1, tmpId2, genres2, s2, new List<string> { "test artist" });
            
            Song[] songs = db.FilterSongs("test album");

            Assert.AreEqual(2, songs.Length);

            Song filtSong1 = songs[0];
            Song filtSong2 = songs[1];

            //Ensure that the first movie is equal to the first movie returned by the filter method (the movies are not returned in a specific order, but the first movie will always be returned first, insider knowledge)
            Assert.AreEqual(song1.Id, filtSong1.Id);
            Assert.AreEqual(song1.Title, filtSong1.Title);
            Assert.AreEqual(song1.Year, filtSong1.Year);
            Assert.AreEqual(song1.BuyPrice, filtSong1.BuyPrice);
            Assert.AreEqual(song1.RentPrice, filtSong1.RentPrice);
            Assert.AreEqual(song1.Album, filtSong1.Album);
            Assert.AreEqual(song1.Artists[0], filtSong1.Artists[0]);
            Assert.AreEqual(song1.Description, filtSong1.Description);

            //Ensure that the second movie is equal to the second movie returned by the filter method
            Assert.AreEqual(song2.Id, filtSong2.Id);
            Assert.AreEqual(song2.Title, filtSong2.Title);
            Assert.AreEqual(song2.Year, filtSong2.Year);
            Assert.AreEqual(song2.BuyPrice, filtSong2.BuyPrice);
            Assert.AreEqual(song2.RentPrice, filtSong2.RentPrice);
            Assert.AreEqual(song2.Album, filtSong2.Album);
            Assert.AreEqual(song2.Artists[0], filtSong2.Artists[0]);
            Assert.AreEqual(song2.Description, filtSong2.Description);
            

            db.DeleteSong(song1.Id, 1);
            db.DeleteSong(song2.Id, 1);
        }*/
    }
}
