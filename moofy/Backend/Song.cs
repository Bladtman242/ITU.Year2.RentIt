using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moofy.Backend {
    public class Song : File {

        public Song(int id) : base(id) {} //Call base constructor

        /// <summary>
        /// The artist of the song
        /// </summary>
        public string Artist { get; set; }
        
        /// <summary>
        /// The album of the song
        /// </summary>
        public string Album { get; set; }

        //Equals override according to http://msdn.microsoft.com/en-us/library/ms173147(v=vs.80).aspx
        public override bool Equals(System.Object obj) {
            // If parameter cannot be cast to Movie return false.
            Song s = obj as Song;
            if ((System.Object)s == null) {
                return false;
            }

            // Return true if the fields match:
            return base.Equals(obj) && (Artist.Equals(s.Artist)) && (Album.Equals(s.Album));
        }

        /// <summary>
        /// A supplement to the normal equals method, which takes a Song parameter instead.
        /// It is recommended by Microsoft, that in addition to implementing Equals(object), Equals(type) should also be implemented for the class's own type in order to enhance performance.
        /// </summary>
        /// <param name="s">The Song to compare</param>
        /// <returns>True if the two objects are equal</returns>
        public bool Equals(Song s) {
            // Return true if the fields match:
            return base.Equals((File)s) && (Artist.Equals(s.Artist)) && (Album.Equals(s.Album));
        }

        public override int GetHashCode() {
            return Id;
        }
    }
}
