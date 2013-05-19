using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace moofy.Backend.Tests
{
    [TestClass]
    public class DBGenreTest
    {
        static DBAccess db;
        public DBGenreTest() {
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
        public void GetGenresValidIdTest()
        {
            //Ensure the file exists (the file with id 1 is a movie)
            Assert.AreNotEqual(db.GetMovie(1), null);
            //With no genres connected to the file
            IList<string> genres = db.GetGenres(1);
            Assert.AreEqual(genres.Count, 0);

            //With one genre connected to the file
            db.AddGenre(1, "Horror");
            genres = db.GetGenres(1);
            Assert.AreEqual(genres.Count, 1);
            Assert.IsTrue(genres.Contains("Horror"));

            //With more than on genre connected to the file
            db.AddGenre(1, "Romance");
            db.AddGenre(1, "Comedy");
            genres = db.GetGenres(1);
            Assert.AreEqual(genres.Count, 3);
            Assert.IsTrue(genres.Contains("Horror"));
            Assert.IsTrue(genres.Contains("Romance"));
            Assert.IsTrue(genres.Contains("Comedy"));
        }

        [TestMethod]
        public void GetGenresInvalidIdTest()
        {
            IList<string> genres = db.GetGenres(-20);
            Assert.AreEqual(genres.Count, 0);
        }

        [TestMethod]
        public void GetGenresNonExistingIdTest()
        {
            IList<string> genres = db.GetGenres(20);
            Assert.AreEqual(genres.Count, 0);
        }

        [TestMethod]
        public void AddGenreExistingTest()
        {
            bool success = db.AddGenre(2, "Romance");
            Assert.IsTrue(success);
        }
        [TestMethod]
        public void AddGenreNonExistingTest()
        {
            bool success = db.AddGenre(2, "Jazz");
            Assert.IsTrue(success);
        }

        [TestMethod]
        public void AddGenreInvalidFileid()
        {
            bool success = db.AddGenre(-1, "FISKEOST");
            Assert.IsFalse(success);
        }
        [TestMethod]
        public void AddGenreNonexestingFile()
        {
            bool success = db.AddGenre(10000, "FISKEOST");
            Assert.IsFalse(success);
        }

        [TestMethod]
        public void AddAllGenresEmptyArray()
        {
            bool success = db.AddAllGenres(1, new string[0]);
            Assert.IsTrue(success);
        }
        [TestMethod]
        public void AddAllGenresArrayWithOneExisting()
        {
            bool success = db.AddAllGenres(2, new string[]{"Romance"});
            Assert.IsTrue(success);
            IList<string> genres = db.GetGenres(2);
            Assert.IsTrue(genres.Contains("Romance"));
        }
        [TestMethod]
        public void AddAllGenresArrayWithOneNonExisting()
        {
            bool success = db.AddAllGenres(2, new string[] { "K-POP" });
            Assert.IsTrue(success);
            IList<string> genres = db.GetGenres(2);
            Assert.IsTrue(genres.Contains("K-POP"));
        }
        [TestMethod]
        public void AddAllGenresArrayWithMultipleNonExisting()
        {
            bool success = db.AddAllGenres(2, new string[] { "K-POP1","K-POP2" });
            Assert.IsTrue(success);
            IList<string> genres = db.GetGenres(2);
            Assert.IsTrue(genres.Contains("K-POP1"));
            Assert.IsTrue(genres.Contains("K-POP2"));
        }
        [TestMethod]
        public void AddAllGenresArrayWithMultipleExisting()
        {
            bool success = db.AddAllGenres(2, new string[] { "Horror", "Comedy" });
            Assert.IsTrue(success);
            IList<string> genres = db.GetGenres(2);
            Assert.IsTrue(genres.Contains("Horror"));
            Assert.IsTrue(genres.Contains("Comedy"));
        }
        [TestMethod]
        public void AddAllGenresArrayWithMultipleMixed()
        {
            bool success = db.AddAllGenres(1, new string[] { "K-POP100", "K-POP2" });
            Assert.IsTrue(success);
            IList<string> genres = db.GetGenres(1);
            Assert.IsTrue(genres.Contains("K-POP100"));
            Assert.IsTrue(genres.Contains("K-POP2"));


        }
        [TestMethod]
        public void AddAllGenresInvalidFile()
        {
            bool success = db.AddAllGenres(-2, new string[] { "Reggae", "Rock" });
            Assert.IsFalse(success);
            IList<string> genres = db.GetGenres(-2);
            Assert.IsFalse(genres.Contains("Reggae"));
            Assert.IsFalse(genres.Contains("Rock"));

        }
        [TestMethod]
        public void AddAllGenresNonExistingFile()
        {
            bool success = db.AddAllGenres(2121, new string[] { "Reggae", "Rock" });
            Assert.IsFalse(success);
            IList<string> genres = db.GetGenres(2121);
            Assert.IsFalse(genres.Contains("Reggae"));
            Assert.IsFalse(genres.Contains("Rock"));

        }

        [TestMethod]
        public void ClearFileGenresValidId()
        {
            bool success = db.ClearFileGenres(2);
            Assert.IsTrue(success);
            Assert.AreEqual(db.GetGenres(2).Count, 0);
        }

        [TestMethod]
        public void ClearFileGenresinValidId()
        {
            bool success = db.ClearFileGenres(-2);
            Assert.IsFalse(success);
        }
        [TestMethod]
        public void ClearFileGenresNonexistingId()
        {
            bool success = db.ClearFileGenres(222);
            Assert.IsFalse(success);
        }
        
    }
}
