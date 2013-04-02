using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moofy.JsonServices {
    public class MovieWrapper {

        public int id {
            get;
            set;
        }

        public string title {
            get;
            set;
        }

        public int release {
            get;
            set;
        }

        public string[] directors {
            get;
            set;
        }

        public string[] genres {
            get;
            set;
        }

        public string description {
            get;
            set;
        }

        public int rentalPrice {
            get;
            set;
        }

        public int purchasePrice {
            get;
            set;
        }

    }
}
