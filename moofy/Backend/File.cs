using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moofy.Backend {
    public abstract class File {
        protected int id;

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
        public string Title { get; set; }

        /// <summary>
        /// The Description of the File
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The Genre of the File
        /// </summary>
        public IList<string> Genres { get; set; }

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
