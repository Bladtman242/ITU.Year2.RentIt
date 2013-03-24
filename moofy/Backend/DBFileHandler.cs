using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.IO;

namespace moofy.Backend {
    public static partial class DBAccess {

        public static string UploadFile(Stream file) {
            using (var fileStream = System.IO.File.Create("C:\\Path\\To\\File")) {
                file.CopyTo(fileStream);
            }
            return "rofl/pops/file";
        }

    }
}
