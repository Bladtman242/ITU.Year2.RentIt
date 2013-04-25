using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moofy.Backend {
    public class Song : File {
        private IList<string> artists;
        /// <summary>
        /// The artist of the song
        /// </summary>
        public IList<string> Artists
        {
            get
            {
                if (artists == null)
                {
                    db.Open();
                    artists = db.GetArtists(this.Id);
                    db.Close();
                }
                return artists;
            }
        }
        /// <summary>
        /// The album of the song
        /// </summary>
        public string Album { get; set; }
    }
}
