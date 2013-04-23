using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.IO;

namespace moofy.Backend {
    public partial class DBAccess {

        /// <summary>
        /// Gets the number of users who have rated this file
        /// </summary>
        /// <param name="fileId">The file to get the number for</param>
        /// <returns>The number of users who have rated this file</returns>
        public int GetNumberOfRatings(int fileId) {
            SqlCommand command = new SqlCommand("SELECT COUNT(*) " +
                                                "FROM UserFileRating " +
                                                "WHERE fid=" + fileId,
                                                connection);
            Object result = command.ExecuteScalar();
            return int.Parse(result.ToString());

        }
        /// <summary>
        /// Get the average rating of a file
        /// </summary>
        /// <param name="fileId">the id of the file</param>
        /// <returns>returns the average rating granted to this file, or 0 if no rating is recorded for this movie</returns>
        public float GetRating(int fileId) {
            SqlCommand command = new SqlCommand("SELECT AVG(rating) " +
                                                "FROM UserFileRating " +
                                                "WHERE fid=" + fileId +
                                                " GROUP BY fid"
                                                , connection);
            Object result = command.ExecuteScalar();
            if (result != null)
                return float.Parse(result.ToString());

            return 0;
        }
       
        /// <summary>
        /// Returns the URI of the file with a given id
        /// </summary>
        /// <param name="fileId">The id of the file to download</param>
        /// <returns>The uri of the file or the empty string if the file does not exist in the database, or null if the user does not have access to download this movie</returns>
        public string DownloadFile(int fileId, int userId) {
            SqlCommand command = new SqlCommand("SELECT fid FROM UserFile WHERE fid=" + fileId + " AND uid=" + userId, connection);
            if (command.ExecuteScalar() == null) return null;

            command.CommandText = "SELECT uri FROM Filez WHERE id =" + fileId;
            Object uri = command.ExecuteScalar();

            if (uri == null) return "";//No file with the given id exists in the database
            else {
                command.CommandText = "UPDATE Filez " +
                                      "SET viewCount = viewCount + 1 " +
                                      "WHERE id = " + fileId;
                command.ExecuteNonQuery();
                return uri.ToString();
            }
        }

        /// <summary>
        /// Uploads a file to disk
        /// </summary>
        /// <param name="file">A stream with the info to be put in the file</param>
        /// <returns>The id of the tmp file added in the database</returns>
        public int UploadFile(Stream file) {
            MultipartParser parser = new MultipartParser(file);
            if (!parser.Success) throw new FormatException("Failed to parse file data");

            int hash = parser.Filename.GetHashCode();
            string ext = "." + parser.Filename.Split('.').Last();

            string pathLocal = "C:\\RentItServices\\Rentit25\\Files\\" + hash + ext;
            string pathHttp = "http://rentit.itu.dk/RentIt25/Files/" + hash + ext;
            Random rand = new Random();

            //Ensure that no other file exists with this name
            while (new FileInfo(pathLocal).Exists) {
                int add = rand.Next();
                pathLocal = pathLocal.Substring(0, pathLocal.Length - ext.Length) + add + ext;

                pathHttp = pathHttp.Substring(0, pathLocal.Length - ext.Length) + add + ext;
            }

            //Create a filestream to the wanted path and copy the contents of the input stream to this 
            using (var fileStream = System.IO.File.Create(pathLocal)) {
                fileStream.Write(parser.FileContents, 0, parser.FileContents.Length);
            }

            SqlCommand command = new SqlCommand("INSERT INTO StagedFile(path) VALUES('" + pathHttp + "')", connection);

            if (command.ExecuteNonQuery() > 0)
            {
                command.CommandText = "SELECT IDENT_CURRENT('StagedFile')";
                return Int32.Parse(command.ExecuteScalar().ToString());
            }

            return 1;
        }

    }
}
