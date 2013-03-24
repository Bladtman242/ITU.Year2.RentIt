using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.IO;

namespace moofy.Backend
{
    class DBFileHandler
    {
        private static SqlConnection connection = new SqlConnection("user id=RentIt25db;" +
                                       "password=ZAQ12wsx;server=rentit.itu.dk;" +
                                       "Trusted_Connection=no;" +
                                       "database=RENTIT25; " +
                                       "connection timeout=30");

        public static string UploadFile(Stream file)
        {
            using (var fileStream = System.IO.File.Create("C:\\Path\\To\\File"))
            {
                file.CopyTo(fileStream);
            }
            return "rofl/pops/file";
        }

    }
}
