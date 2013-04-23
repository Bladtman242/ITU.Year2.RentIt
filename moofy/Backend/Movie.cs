using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moofy.Backend {
    public class Movie : File {
        private IList<string> directors;
        /// <summary>
        /// The director of the movie
        /// </summary>
        public IList<string> Directors {
            get
            {
                if (directors == null)
                {
                    db.Open();
                    directors = db.GetDirectors(this.Id);
                    db.Close();
                }
                return directors;
            }
       }
    }
}
