using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.IO;

namespace moofy.Backend {
    public partial class DBAccess {

        public int UploadFile(Stream file) {
            string path = "C:\\RENTIT25\\" + file.GetHashCode();
            Random rand = new Random();

            //Ensure that no other file exists with this name
            while (new FileInfo(path).Exists)
            {
                path = path + rand.Next();
            }

            //Create a filestream to the wanted path and copy the contents of the input stream to this 
            using (var fileStream = System.IO.File.Create(path))
            {
                file.CopyTo(fileStream);
            }

            SqlCommand command = new SqlCommand("INSERT INTO StagedFile(path) VALUES('" + path + "')", connection);

            if (command.ExecuteNonQuery() > 0)
            {
                command.CommandText = "SELECT IDENT_CURRENT('StagedFile')";
                return Int32.Parse(command.ExecuteScalar().ToString());
            }
            return -1;
        }

    }
}
