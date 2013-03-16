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

        IList<string> Genre {
            get;
            set;
        }

        short Year {
            get;
            set;
        }

        string URI {
            get;
            set;
        }

        float RentPrice {
            get;
            set;
        }

        float BuyPrice {
            get;
            set;
        }
    }
}
