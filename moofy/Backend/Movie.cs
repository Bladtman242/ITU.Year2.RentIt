using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moofy.Backend {
    public class Movie : File {
        /// <summary>
        /// The director of the movie
        /// </summary>
        public string Director { get; set; }
    }
}
