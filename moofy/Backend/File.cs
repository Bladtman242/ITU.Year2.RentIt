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
        public int BuyPrice { get; set; }

        /// <summary>
        /// The cost of the File when rented
        /// </summary>
        public int RentPrice { get; set; }

        protected static void ByTitle(File[] files) {
            Array.Sort(files, (f1, f2) => f1.Title.CompareTo(f2.Title));
        }

        protected static void ByDescription(File[] files) {
            Array.Sort(files, (f1, f2) => f1.Description.CompareTo(f2.Description));
        }

        protected static void ByGenre(File[] files) {
            Array.Sort(files, (f1, f2) => {
                String[] arr1 = f1.genres.ToArray();
                String[] arr2 = f2.genres.ToArray();
                Array.Sort(arr1);
                Array.Sort(arr2);
                return arr1[0].CompareTo(arr2[0]); //This is a pretty useless (and slow) sort.
            });
        }

        protected static void ByYear(File[] files) {
            Array.Sort(files, (f1, f2) => f1.Year.CompareTo(f2.Year));
        }

        protected static void ByBuyPrice(File[] files) {
            Array.Sort(files, (f1, f2) => f1.BuyPrice.CompareTo(f2.BuyPrice));
        }

        protected static void ByRentPrice(File[] files) {
            Array.Sort(files, (f1, f2) => f1.RentPrice.CompareTo(f2.RentPrice));
        }

        public static void SortBy(File[] files, String property) {
            //should probably be done with enums, or by a completely different pattern (visitor?)
            switch (property.ToLower()) {
                case "title":
                    ByTitle(files);
                    break;
                case "description" :
                    ByDescription(files);
                    break;
                case "genre":
                    ByGenre(files);
                    break;
                case "release":
                case "year":
                    ByYear(files);
                    break;
                default:
                    throw new ArgumentException(property + " is undefined");
            }
        }
    }
}