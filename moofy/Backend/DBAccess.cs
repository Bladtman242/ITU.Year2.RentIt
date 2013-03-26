using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace moofy.Backend {
    public partial class DBAccess {
        protected SqlConnection connection;
       
        public DBAccess() {
            connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]);//Skal evt. ændres til WebConfigurationManager.AppSettings for at virke med webservicen deployed på rentit.itu.dk
        }

        public void Open() {
            connection.Open();
        }

        public void Close() {
            connection.Close();
        }
    }
}
