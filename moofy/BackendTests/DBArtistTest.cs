using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace moofy.Backend.Tests
{
    [TestClass]
    public class DBArtistTest
    {
        static DBAccess db;
        public DBArtistTest()
        {
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
        public void GetArtistsValidIdTest()
        {
            Assert.AreNotEqual(db.GetSong(2), null);
            IList<string> artists = db.GetArtists(2);
            Assert.AreEqual(artists.Count, 1);

            //One artist connected to the file
            db.AddArtist(2, "Lonely Island");
            artists = db.GetArtists(2);
            Assert.AreEqual(artists.Count, 2);
            Assert.IsTrue(artists.Contains("Lonely Island"));

            //With more than on artist connected to the file
            db.AddArtist(2, "Sting");
            db.AddArtist(2, "George Michael");
            artists = db.GetArtists(2);
            Assert.AreEqual(artists.Count, 4);
            Assert.IsTrue(artists.Contains("Lonely Island"));
            Assert.IsTrue(artists.Contains("Sting"));
            Assert.IsTrue(artists.Contains("George Michael"));
        }

        [TestMethod]
        public void GetArtistsInvalidIdTest()
        {
            IList<string> artists = db.GetArtists(-20);
            Assert.AreEqual(artists.Count, 0);
        }

        [TestMethod]
        public void GetArtistsNonExistingIdTest()
        {
            IList<string> artists = db.GetArtists(20);
            Assert.AreEqual(artists.Count, 0);
        }

        [TestMethod]
        public void AddArtistExistingTest()
        {
            bool success = db.AddArtist(2, "Sting");
            Assert.IsTrue(success);
        }
        [TestMethod]
        public void AddArtistNonExistingTest()
        {
            bool success = db.AddArtist(2, "Police");
            Assert.IsTrue(success);
        }

        [TestMethod]
        public void AddArtistInvalidFileid()
        {
            bool success = db.AddArtist(-1, "Elvis");
            Assert.IsFalse(success);
        }
        [TestMethod]
        public void AddArtistNonexestingFile()
        {
            bool success = db.AddArtist(10000, "Elvis");
            Assert.IsFalse(success);
        }

        [TestMethod]
        public void AddAllArtistsEmptyArray()
        {
            bool success = db.AddAllArtists(2, new string[0]);
            Assert.IsTrue(success);
        }
        [TestMethod]
        public void AddAllArtistsArrayWithOneExisting()
        {
            bool success = db.AddAllArtists(4, new string[] { "Sting" });
            Assert.IsTrue(success);
            IList<string> artists = db.GetArtists(4);
            Assert.IsTrue(artists.Contains("Sting"));
        }
        [TestMethod]
        public void AddAllArtistsArrayWithOneNonExisting()
        {
            bool success = db.AddAllArtists(4, new string[] { "Iron Maiden" });
            Assert.IsTrue(success);
            IList<string> artists = db.GetArtists(4);
            Assert.IsTrue(artists.Contains("Iron Maiden"));
        }
        [TestMethod]
        public void AddAllArtistsArrayWithMultipleNonExisting()
        {
            bool success = db.AddAllArtists(2, new string[] { "Elvis Costello", "ABBA" });
            Assert.IsTrue(success);
            IList<string> artists = db.GetArtists(2);
            Assert.IsTrue(artists.Contains("Elvis Costello"));
            Assert.IsTrue(artists.Contains("ABBA"));
        }
        [TestMethod]
        public void AddAllArtistsArrayWithMultipleExisting()
        {
            bool success = db.AddAllArtists(4, new string[] { "ABBA", "Elvis Costello" });
            Assert.IsTrue(success);
            IList<string> artists = db.GetArtists(4);
            Assert.IsTrue(artists.Contains("ABBA"));
            Assert.IsTrue(artists.Contains("Elvis Costello"));
        }
        [TestMethod]
        public void AddAllArtistsArrayWithMultipleMixed()
        {
            bool success = db.AddAllArtists(4, new string[] { "Police", "Bruce Springsteen" });
            Assert.IsTrue(success);
            IList<string> artists = db.GetArtists(4);
            Assert.IsTrue(artists.Contains("Police"));
            Assert.IsTrue(artists.Contains("Bruce Springsteen"));


        }
        [TestMethod]
        public void AddAllArtistsInvalidFile()
        {
            bool success = db.AddAllArtists(-2, new string[] { "Elvis", "Tool" });
            Assert.IsFalse(success);
            IList<string> artists = db.GetArtists(-2);
            Assert.IsFalse(artists.Contains("Elvis"));
            Assert.IsFalse(artists.Contains("Tool"));

        }
        [TestMethod]
        public void AddAllArtistsNonExistingFile()
        {
            bool success = db.AddAllArtists(2121, new string[] { "Elvis", "Tool" });
            Assert.IsFalse(success);
            IList<string> artists = db.GetArtists(2121);
            Assert.IsFalse(artists.Contains("Elvis"));
            Assert.IsFalse(artists.Contains("Tool"));

        }

        [TestMethod]
        public void ClearSongArtistsValidId()
        {
            bool success = db.ClearSongArtists(2);
            Assert.IsTrue(success);
            Assert.AreEqual(db.GetArtists(2).Count, 0);
        }

        [TestMethod]
        public void ClearSongArtistsinValidId()
        {
            bool success = db.ClearSongArtists(-2);
            Assert.IsFalse(success);
        }
        [TestMethod]
        public void ClearSongArtistsNonexistingId()
        {
            bool success = db.ClearSongArtists(222);
            Assert.IsFalse(success);
        }
    }
}
