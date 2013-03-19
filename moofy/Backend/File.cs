using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moofy.Backend {
    public abstract class File {
        //A set of backing fields
        protected int id;
        protected short year;
        protected string title, description, uri;
        protected float buyPrice, rentPrice;
        protected IList<string> genres;

        public File(int id) {
            this.id = id;
        }

        /// <summary>
        /// The Id of the File
        /// </summary>
        public int Id {
            get { return id; }
        }

        /// <summary>
        /// The Title of the File
        /// </summary>
        public string Title {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// The Description of the File
        /// </summary>
        public string Description {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// The Genre of the File
        /// </summary>
        public IList<string> Genres {
            get { return genres; }
            set { genres = value; }
        }

        /// <summary>
        /// The Year of the File
        /// </summary>
        public short Year {
            get { return year; }
            set { year = value; }
        }

        /// <summary>
        /// The location of the File as a URI
        /// </summary>
        public string Uri {
            get { return uri; }
            set { uri = value; }
        }

        /// <summary>
        /// The cost of the File, when bought
        /// </summary>
        public float BuyPrice {
            get { return buyPrice; }
            set { buyPrice = value; }
        }

        /// <summary>
        /// The cost of the File when rented
        /// </summary>
        public float RentPrice {
            get { return rentPrice; }
            set { rentPrice = value; }
        }

        //Equals override according to http://msdn.microsoft.com/en-us/library/ms173147(v=vs.80).aspx
        public override bool Equals(System.Object obj) {
            // If parameter is null return false.
            if (obj == null) {
                return false;
            }

            // If parameter cannot be cast to File return false.
            File f = obj as File;
            if ((System.Object)f == null) {
                return false;
            }

            // Return true if the fields match:
            return (Id == f.Id)
                    && (Title.Equals(f.Title))
                    && (Description.Equals(f.Description))
                    && (Genres.Equals(f.Genres))
                    && (Year == f.Year)
                    && (Uri.Equals(f.Uri))
                    && (BuyPrice == f.BuyPrice)
                    && (RentPrice == f.RentPrice);
        }

        /// <summary>
        /// A supplement to the normal equals method, which takes a File parameter for performance.
        /// It is recommended by Microsoft, that in addition to implementing Equals(object), Equals(type) should also be implemented for the class's own type in order to enhance performance.
        /// </summary>
        /// <param name="f">The File to compare</param>
        /// <returns>True if the two objects are equal</returns>
        public bool Equals(File f) {
            // If parameter is null return false:
            if ((object)f == null) {
                return false;
            }

            // Return true if the fields match:
            return (Id == f.Id)
                    && (Title.Equals(f.Title))
                    && (Description.Equals(f.Description))
                    && (Genres.Equals(f.Genres))
                    && (Year == f.Year)
                    && (Uri.Equals(f.Uri))
                    && (BuyPrice == f.BuyPrice)
                    && (RentPrice == f.RentPrice);
        }

        public override int GetHashCode() {
            return Id;
        }
    }
}
