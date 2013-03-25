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
        public void FilterMovieTest()
        {
            
            Movie[] movies = db.FilterMovies("niclas");
            
            Assert.IsTrue(movies.Length > 0);
            Movie mov = movies[0];
            Assert.AreEqual(1, mov.Id);
            Assert.AreEqual("Life of a Small Son", mov.Title);
        }
    }
}
