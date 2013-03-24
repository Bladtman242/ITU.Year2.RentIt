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
            connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]);
        }

        public void Open() {
            connection.Open();
        }

        public void Close() {
            connection.Close();
        }
    }
}
