using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace moofy.Backend {
    public static partial class DBAccess {
        private static SqlConnection connection = new SqlConnection("user id=RentIt25db;" +
                                       "password=ZAQ12wsx;server=rentit.itu.dk;" +
                                       "Trusted_Connection=no;" +
                                       "database=RENTIT25; " +
                                       "connection timeout=30");
    }
}
