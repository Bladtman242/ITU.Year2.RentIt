using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moofy.Backend {
    public class Movie : File {
        public Movie(int id) : base(id) {} //Call base constructor

        public string Director { get; set; }

        //Equals override according to http://msdn.microsoft.com/en-us/library/ms173147(v=vs.80).aspx
        public override bool Equals(System.Object obj) {
            // If parameter cannot be cast to Movie return false.
            Movie m = obj as Movie;
            if ((System.Object)m == null) {
                return false;
            }

            // Return true if the fields match:
            return base.Equals(obj) && (Director == m.Director);
        }

        public bool Equals(Movie m) {
            // Return true if the fields match:
            return base.Equals((File)m) && (Director == m.Director);
        }

        public override int GetHashCode() {
            return Id;
        }
    }
}
