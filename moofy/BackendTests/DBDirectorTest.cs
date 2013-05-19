using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace moofy.Backend.Tests
{
    [TestClass]
    public class DBDirectorTest
    {
        static DBAccess db;
        public DBDirectorTest() {
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
        public void GetDirectorsValidIdTest()
        {
            //Ensure the file exists (the file with id 1 is a movie)
            Assert.AreNotEqual(db.GetMovie(1), null);
            //With no directors connected to the file
            IList<string> directors = db.GetDirectors(1);
            Assert.AreEqual(directors.Count, 1);

            //With one director connected to the file
            db.AddDirector(1, "Tarantino");
            directors = db.GetDirectors(1);
            Assert.AreEqual(directors.Count, 2);
            Assert.IsTrue(directors.Contains("Tarantino"));

            //With more than one director connected to the file
            db.AddDirector(1, "Test1");
            db.AddDirector(1, "Test2");
            directors = db.GetDirectors(1);
            Assert.AreEqual(directors.Count, 4);
            Assert.IsTrue(directors.Contains("Tarantino"));
            Assert.IsTrue(directors.Contains("Test1"));
            Assert.IsTrue(directors.Contains("Test2"));
        }

        [TestMethod]
        public void GetDirectorsInvalidIdTest()
        {
            IList<string> directors = db.GetDirectors(-20);
            Assert.AreEqual(directors.Count, 0);
        }

        [TestMethod]
        public void GetDirectorsNonExistingIdTest()
        {
            IList<string> directors = db.GetDirectors(20);
            Assert.AreEqual(directors.Count, 0);
        }

        [TestMethod]
        public void AddDirectorExistingTest()
        {
            bool success = db.AddDirector(3, "Test1");
            Assert.IsTrue(success);
        }
        [TestMethod]
        public void AddDirectorNonExistingTest()
        {
            bool success = db.AddDirector(1, "Test3");
            Assert.IsTrue(success);
        }

        [TestMethod]
        public void AddDirectorInvalidMovieid()
        {
            bool success = db.AddDirector(-1, "FailTest");
            Assert.IsFalse(success);
        }

        [TestMethod]
        public void AddDirectorNonexestingMovie()
        {
            bool success = db.AddDirector(10000, "FailTest");
            Assert.IsFalse(success);
        }

        [TestMethod]
        public void AddAllDirectorsEmptyArray()
        {
            bool success = db.AddAllDirectors(1, new string[0]);
            Assert.IsTrue(success);
        }
        [TestMethod]
        public void AddAllDirectorsArrayWithOneExisting()
        {
            bool success = db.AddAllDirectors(3, new string[] { "Test2" });
            Assert.IsTrue(success);
            IList<string> directors = db.GetDirectors(3);
            Assert.IsTrue(directors.Contains("Test2"));
        }
        [TestMethod]
        public void AddAllDirectorsArrayWithOneNonExisting()
        {
            bool success = db.AddAllDirectors(1, new string[] { "Test4" });
            Assert.IsTrue(success);
            IList<string> directors = db.GetDirectors(1);
            Assert.IsTrue(directors.Contains("Test4"));
        }
        [TestMethod]
        public void AddAllDirectorsArrayWithMultipleNonExisting()
        {
            bool success = db.AddAllDirectors(1, new string[] { "Test5", "Test6" });
            Assert.IsTrue(success);
            IList<string> directors = db.GetDirectors(1);
            Assert.IsTrue(directors.Contains("Test5"));
            Assert.IsTrue(directors.Contains("Test6"));
        }
        [TestMethod]
        public void AddAllDirectorsArrayWithMultipleExisting()
        {
            bool success = db.AddAllDirectors(3, new string[] { "Test5", "Test6" });
            Assert.IsTrue(success);
            IList<string> directors = db.GetDirectors(3);
            Assert.IsTrue(directors.Contains("Test5"));
            Assert.IsTrue(directors.Contains("Test6"));
        }
        [TestMethod]
        public void AddAllDirectorsArrayWithMultipleMixed()
        {
            bool success = db.AddAllDirectors(3, new string[] { "Test4", "Test7" });
            Assert.IsTrue(success);
            IList<string> directors = db.GetDirectors(3);
            Assert.IsTrue(directors.Contains("Test4"));
            Assert.IsTrue(directors.Contains("Test7"));


        }
        [TestMethod]
        public void AddAllDirectorsInvalidMovie()
        {
            bool success = db.AddAllDirectors(-2, new string[] { "Test8", "Test9" });
            Assert.IsFalse(success);
            IList<string> directors = db.GetDirectors(-2);
            Assert.IsFalse(directors.Contains("Test8"));
            Assert.IsFalse(directors.Contains("Test9"));

        }
        [TestMethod]
        public void AddAllDirectorsNonExistingMovie()
        {
            bool success = db.AddAllDirectors(2121, new string[] { "Test10", "Test11" });
            Assert.IsFalse(success);
            IList<string> directors = db.GetDirectors(2121);
            Assert.IsFalse(directors.Contains("Test10"));
            Assert.IsFalse(directors.Contains("Test11"));

        }

        [TestMethod]
        public void ClearMovieDirectorsValidId()
        {
            bool success = db.ClearMovieDirectors(3);
            Assert.IsTrue(success);
            Assert.AreEqual(db.GetDirectors(3).Count, 0);
        }

        [TestMethod]
        public void ClearMovieDirectorsinValidId()
        {
            bool success = db.ClearMovieDirectors(-2);
            Assert.IsFalse(success);
        }
        [TestMethod]
        public void ClearMovieDirectorsNonexistingId()
        {
            bool success = db.ClearMovieDirectors(222);
            Assert.IsFalse(success);
        }


    }
}
