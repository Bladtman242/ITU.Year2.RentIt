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
            
        }

        [ClassCleanup()]
        public static void CleanUp() {
            db.Close();
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
            int songId = db.CreateSong(1, tmpId, "test", 1900, 1000, rent, "testalbum", "testartist", new string[] { "Horror" }, "description");

            //Create a user to buy the movie, who can afford it
            int userId = db.AddUser(new User()
            {
                Name = "TestUser",
                Username = "TestUser",
                Balance = rent,
                Email = "testuser@buy.com",
                Password = "test"
            });

            Assert.IsTrue(db.RentSong(songId, userId,3));

            //Get the user from the database (required to be able to load the newly added file to his owned files)
            User actualUser = db.GetUser(userId);
            //Assert that the movie is added to movies owned by the user
            Assert.IsTrue(actualUser.Purchases.ElementAt(0).File.Id == songId);
            TimeSpan ts = DateTime.Now.AddDays(3) - actualUser.Purchases.ElementAt(0).EndTime;
            //Assert that the expiration time is within 3days and 10minutes, the 10minutes are added as a buffer for the time it takes to run the code inbetween the RentMovie call and the creation of ts.
            //A 10minute discrepency can be accepted in the system
            Assert.IsTrue(ts.TotalMinutes < 1);

            db.DeleteUser(userId);
            db.DeleteSong(songId, 1);


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
            int songId = db.CreateSong(1, tmpId, "test", 1900, buy, 10, "tesalbum", "testartist", new string[] { "Horror" }, "description");

            //Create a user to buy the movie, who can afford it
            int userId = db.AddUser(new User(){
                    Name = "TestUser",
                    Username = "TestUser",
                    Balance = buy,
                    Email = "testuser@buy.com",
                    Password = "test"
            });

            Assert.IsTrue(db.PurchaseSong(songId, userId));
            //Get the user from the database (required to be able to load the newly added file to his owned files)
            User actualUser = db.GetUser(userId);
            //Assert that the movie is added to movies owned by the user
            Assert.IsTrue(actualUser.Purchases.ElementAt(0).File.Id == songId);
            //Assert that the date of expiration is set to max, comparing strings as DateTimes compare is an reference equality
            Assert.AreEqual(actualUser.Purchases.ElementAt(0).EndTime.ToString(), DateTime.MaxValue.ToString());

            db.DeleteUser(userId);
            db.DeleteSong(songId, 1);
            

        }
       
        
        [TestMethod]
        public void CreateAndGetSongTest()
        {
            
            MemoryStream s = new MemoryStream();
            s.WriteByte(5);
            int tmpId = db.UploadFile(s);

            //Values that will be given to the CreateMovie method
            string title = "test";
            short year = 2000;
            int buy = 10;
            int rent = 1;
            string album = "test album";
            string artist = "test artist";
            string[] genres = new string[] { "Horror" };
            string description = "test movie description";

            int id = db.CreateSong(1, tmpId, title, year, buy, rent, album, artist, genres, description);
            
            //The actual values in the database
            Song actual = db.GetSong(id);

            Assert.AreEqual(actual.Id, id);
            Assert.AreEqual(actual.Year, year);
            Assert.AreEqual(actual.Title, title);
            Assert.AreEqual(actual.BuyPrice, buy);
            Assert.AreEqual(actual.RentPrice, rent);
            Assert.AreEqual(actual.Album, album);
            Assert.AreEqual(actual.Artist, artist);
            Assert.AreEqual(actual.Description, description);

            db.DeleteSong(id, 1);
        }
        
        [TestMethod]
        public void FilterSongTest()
        {
            MemoryStream s = new MemoryStream();
            s.WriteByte(5);
            int tmpId1 = db.UploadFile(s);
            int tmpId2 = db.UploadFile(s);

            string title1 = "test1";
            short year1 = 2000;
            int buy1 = 10;
            int rent1 = 1;
            string album1 = "test album";
            string artist1 = "test artist";
            string[] genres1 = new string[] { "Horror" };
            string description1 = "test movie description";

            int id1 = db.CreateSong(1, tmpId1, title1, year1, buy1, rent1, album1, artist1, genres1, description1);

            string title2 = "test album";
            short year2 = 2040;
            int buy2 = 1022;
            int rent2 = 100;
            string album2 = "test albummmmmmm";
            string artist2 = "test artist";
            string[] genres2 = new string[] { "Comedy" };
            string description2 = "test movie description22";

            int id2 = db.CreateSong(1, tmpId2, title2, year2, buy2, rent2, album2, artist2, genres2, description2);
            
            Song[] songs = db.FilterSongs("test album");

            Assert.AreEqual(2, songs.Length);

            Song song1 = songs[0];
            Song song2 = songs[1];

            //Ensure that the first movie is equal to the first movie returned by the filter method (the movies are not returned in a specific order, but the first movie will always be returned first, insider knowledge)
            Assert.AreEqual(id1, song1.Id);
            Assert.AreEqual(title1, song1.Title);
            Assert.AreEqual(year1, song1.Year);
            Assert.AreEqual(buy1, song1.BuyPrice);
            Assert.AreEqual(rent1, song1.RentPrice);
            Assert.AreEqual(album1, song1.Album);
            Assert.AreEqual(artist1, song1.Artist);
            Assert.AreEqual(description1, song1.Description);

            //Ensure that the second movie is equal to the second movie returned by the filter method
            Assert.AreEqual(id2, song2.Id);
            Assert.AreEqual(title2, song2.Title);
            Assert.AreEqual(year2, song2.Year);
            Assert.AreEqual(buy2, song2.BuyPrice);
            Assert.AreEqual(rent2, song2.RentPrice);
            Assert.AreEqual(album2, song2.Album);
            Assert.AreEqual(artist2, song2.Artist);
            Assert.AreEqual(description2, song2.Description);

            db.DeleteSong(id1, 1);
            db.DeleteSong(id2, 1);
        }
    }
}
