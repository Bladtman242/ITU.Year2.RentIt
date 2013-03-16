using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moofy.Backend {
    public interface File {
        int Id {
            get;
        }

        string Title {
            get;
            set;
        }

        string Description {
            get;
            set;
        }

        IList<string> Genres {
            get;
            set;
        }

        short Year {
            get;
            set;
        }

        string Uri {
            get;
            set;
        }

        float BuyPrice {
            get;
            set;
        }

        float RentPrice {
            get;
            set;
        }
    }
}
