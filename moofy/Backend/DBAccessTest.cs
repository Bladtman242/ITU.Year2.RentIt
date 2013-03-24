using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace moofy.Backend {
    public class DBAccessTest : DBAccess {
        public DBAccessTest() : base() {
            connection = new SqlConnection("user id=RentIt25db;" +
                                       "password=ZAQ12wsx;server=rentit.itu.dk;" +
                                       "Trusted_Connection=no;" +
                                       "database=RENTIT25TEST; " +
                                       "connection timeout=30");
        }
    }
}
