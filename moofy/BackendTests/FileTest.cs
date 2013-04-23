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
        /// Tests whether the Genres are retrieved correctly in the File object
        /// </summary>
        [TestMethod]
        public void GenresTest() {
            //Upload a file
            MemoryStream s = new MemoryStream();
            s.WriteByte(5);
            int tmpId = db.UploadFile(s);
            //Create a movie with genres horror, comedy and action
            IList<string> expectedGenres = new List<string> { "Horror", "Comedy", "Action" };
            Movie mov = new Movie()
            {
                Title = "title",
                Year = 1900,
                BuyPrice = 1000,
                RentPrice = 100,
                Description = "description"
            };
            Movie movie = db.CreateMovie(1, tmpId, expectedGenres, mov, new List<string>{"director"});
            
            //Retrieve the movie from the database and check that the genres match the inserted
            Movie actMovie = db.GetMovie(movie.Id);
            IList<string> actualGenres = actMovie.Genres;
            Assert.AreEqual(expectedGenres.Count, actualGenres.Count);
            foreach (string genre in expectedGenres) {
                Assert.IsTrue(actualGenres.Contains(genre));
            }

            db.DeleteMovie(movie.Id, 1);
        }
    }
}
