using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moofy.Backend {
    public abstract class File {
        protected int id;
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
                if ((object)genres == null) {
                    db.Open();
                    genres = db.GetGenres(this.Id);
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
        /// The cost of the File, when bought
        /// </summary>
        public float BuyPrice { get; set; }

        /// <summary>
        /// The cost of the File when rented
        /// </summary>
        public float RentPrice { get; set; }

    }
}
