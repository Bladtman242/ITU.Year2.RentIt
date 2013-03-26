using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;

namespace moofy.Backend.Tests {
    [TestClass]
    public class FileTest {
        static DBAccess db;

        public FileTest() {
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
        public void GenresTest() {
            //Upload a file
            MemoryStream s = new MemoryStream();
            s.WriteByte(5);
            int tmpId = db.UploadFile(s);
            //Create a movie with genres horror, comedy and action
            IList<string> expectedGenres = new List<string> { "Horror", "Comedy", "Action" };
            int movieId = db.CreateMovie(1, tmpId, "title", 1900, 1000, 100, "director", expectedGenres, "description");
            
            //Retrieve the movie from the database and check that the genres match the inserted
            Movie movie = db.GetMovie(movieId);
            IList<string> actualGenres = movie.Genres;
            Assert.AreEqual(expectedGenres.Count, actualGenres.Count);
            foreach (string genre in expectedGenres) {
                Assert.IsTrue(actualGenres.Contains(genre));
            }

            db.DeleteMovie(movieId, 1);
        }
    }
}
