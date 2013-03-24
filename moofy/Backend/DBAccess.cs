using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace moofy.Backend {
    public partial class DBAccess {
        protected SqlConnection connection;
       
        public DBAccess() {
            connection = new SqlConnection("user id=RentIt25db;" +
                                       "password=ZAQ12wsx;server=rentit.itu.dk;" +
                                       "Trusted_Connection=no;" +
                                       "database=RENTIT25; " +
                                       "connection timeout=30");
        }

        public void Open() {
            connection.Open();
        }

        public void Close() {
            connection.Close();
        }
    }
}
