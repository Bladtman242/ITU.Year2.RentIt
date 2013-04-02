using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moofy.Backend {
    public abstract class File {
        protected int id;

        private float? averageRating;
        private int? numberOfRatings;
        private IList<string> genres;
        private DBAccess db;

        public File() {
            db = new DBAccess();
        }

        /// <summary>
        /// The Id of the File.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Title of the File
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The Description of the File
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The Genre of the File
        /// </summary>
        public IList<string> Genres {
            get {
                if (genres == null) {
                    db.Open();
                    genres = db.GetGenres(this.Id);
                    List<string> l = (List<string>)genres;
                    l.Sort();
                    genres = (IList<string>)l;
                    db.Close();
                }
                return genres;
            }
        }

        /// <summary>
        /// The Year of the File
        /// </summary>
        public short Year { get; set; }

        /// <summary>
        /// The location of the File as a URI
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// The location of the files cover picture as a URI
        /// </summary>
        public string CoverUri { get; set; }

        /// <summary>
        /// The cost of the File, when bought
        /// </summary>
        public int BuyPrice { get; set; }

        /// <summary>
        /// The cost of the File when rented
        /// </summary>
        public int RentPrice { get; set; }

        /// <summary>
        /// The Average rating of the File
        /// </summary>
        public float AverageRating {
            get {
                if (averageRating == null) {
                    db.Open();
                    averageRating = db.GetRating(this.Id);
                    db.Close();
                }
                return (float)averageRating;
            }
        }

        /// <summary>
        /// The number of ratings of the File
        /// </summary>
        public int NumberOfRatings {
            get {
                if (numberOfRatings == null) {
                    db.Open();
                    numberOfRatings = db.GetNumberOfRatings(this.Id);
                    db.Close();
                }
                return (int)numberOfRatings;
            }
        }

    }
}