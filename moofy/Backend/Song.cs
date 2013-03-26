using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moofy.Backend {
    public class Song : File {
        /// <summary>
        /// The artist of the song
        /// </summary>
        public string Artist { get; set; }
        
        /// <summary>
        /// The album of the song
        /// </summary>
        public string Album { get; set; }
    }
}
