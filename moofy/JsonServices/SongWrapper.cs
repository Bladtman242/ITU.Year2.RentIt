using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moofy.JsonServices {
    public class SongWrapper {

        public string title {
            get;
            set;
        }

        public int release {
            get;
            set;
        }

        public string artist {
            get;
            set;
        }

        public string album {
            get;
            set;
        }

        public string[] genres {
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
