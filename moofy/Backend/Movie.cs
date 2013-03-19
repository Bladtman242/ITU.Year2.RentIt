using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moofy.Backend {
    public class Movie : File {
        public Movie(int id) : base(id) {} //Call base constructor

        /// <summary>
        /// The director of the movie
        /// </summary>
        public string Director { get; set; }

        //Equals override according to http://msdn.microsoft.com/en-us/library/ms173147(v=vs.80).aspx
        public override bool Equals(System.Object obj) {
            // If parameter cannot be cast to Movie return false.
            Movie m = obj as Movie;
            if ((System.Object)m == null) {
                return false;
            }

            // Return true if the fields match:
            return base.Equals(obj) && (Director.Equals(m.Director));
        }

        /// <summary>
        /// A supplement to the normal equals method, which takes a Movie parameter for performance.
        /// It is recommended by Microsoft, that in addition to implementing Equals(object), Equals(type) should also be implemented for the class's own type in order to enhance performance.
        /// </summary>
        /// <param name="m">The Movie to compare</param>
        /// <returns>True if the two objects are equal</returns>
        public bool Equals(Movie m) {
            // Return true if the fields match:
            return base.Equals((File)m) && (Director.Equals(m.Director));
        }

        public override int GetHashCode() {
            return Id;
        }
    }
}
