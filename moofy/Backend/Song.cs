using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moofy.Backend {
    public class Song : File {

        public Song(int id) : base(id) {} //Call base constructor

        public string Artist { get; set; }
        
        public string Album { get; set; }

        //Equals override according to http://msdn.microsoft.com/en-us/library/ms173147(v=vs.80).aspx
        public override bool Equals(System.Object obj) {
            // If parameter cannot be cast to Movie return false.
            Song s = obj as Song;
            if ((System.Object)s == null) {
                return false;
            }

            // Return true if the fields match:
            return base.Equals(obj) && (Artist == s.Artist) && (Album == s.Album);
        }

        public bool Equals(Song s) {
            // Return true if the fields match:
            return base.Equals((File)s) && (Artist == s.Artist) && (Album == s.Album);
        }

        public override int GetHashCode() {
            return Id;
        }
    }
}
