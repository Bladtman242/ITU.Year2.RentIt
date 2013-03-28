using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using moofy.Backend;

namespace moofy.JsonServices {
    public partial class MoofyServices : IMovieService {
        private DBAccess db = new DBAccess();
    }
}
