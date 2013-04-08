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
        public int GetNumberOfRatings(int fileId)
        {
            SqlCommand command = new SqlCommand("SELECT COUNT(*) "+
                                                "FROM UserFileRating " +
                                                "WHERE fid=" + fileId ,
                                                connection);
            Object result = command.ExecuteScalar();
            return int.Parse(result.ToString());

        }
        /// <summary>
        /// Get the average rating of a file
        /// </summary>
        /// <param name="fileId">the id of the file</param>
        /// <returns>returns the average rating granted to this file, or 0 if no rating is recorded for this movie</returns>
        public float GetRating(int fileId)
        {
            SqlCommand command = new SqlCommand("SELECT AVG(rating) " +
                                                "FROM UserFileRating " +
                                                "WHERE fid=" + fileId +
                                                " GROUP BY fid"
                                                , connection);
            Object result = command.ExecuteScalar();
            if(result != null )
                return float.Parse(result.ToString());

            return 0;
        }
        /// <summary>
        /// Gets all genres for a file
        /// </summary>
        /// <param name="fileId">id of the file to get</param>
        /// <returns>a list of all the genre names</returns>
        public IList<string> GetGenres(int fileId)
        {
            SqlCommand command = new SqlCommand("SELECT name FROM Genre, GenreFile "+
                                                "WHERE GenreFile.fid =" + fileId +" "+
                                                "AND GenreFile.gid = Genre.id"
                                                , connection);

            SqlDataReader reader = command.ExecuteReader();
            IList<string> genres = new List<string>();
            while (reader.Read())
            {
                genres.Add(reader["name"].ToString());
            }
            
            return genres;
        }
        /// <summary>
        /// Returns the URI of the file with a given id
        /// </summary>
        /// <param name="fileId">The id of the file to download</param>
        /// <returns>The uri of the file or the empty string if the file does not exist in the database, or null if the user does not have access to download this movie</returns>
        public string DownloadFile(int fileId, int userId)
        {
            SqlCommand command = new SqlCommand("SELECT fid FROM UserFile WHERE fid="+fileId+" AND uid="+userId , connection);
            if (command.ExecuteScalar() == null) return null;

            command.CommandText = "SELECT uri FROM Filez WHERE id =" + fileId;
            Object uri = command.ExecuteScalar();

            if (uri == null) return "";//No file with the given id exists in the database
            else return uri.ToString();
        }

        /// <summary>
        /// Uploads a file to disk
        /// </summary>
        /// <param name="file">A stream with the info to be put in the file</param>
        /// <returns>The id of the tmp file added in the database</returns>
        public int UploadFile(Stream file) {
            int hash = file.GetHashCode();
            string pathLocal = "C:\\RentItServices\\Rentit25\\Files\\" + hash;
            string pathHttp = "http://rentit.itu.dk/RentIt25/Files/" + hash;
            Random rand = new Random();

            //Ensure that no other file exists with this name
            while (new FileInfo(pathLocal).Exists)
            {
                int add = rand.Next();
                pathLocal = pathLocal + add;
                pathHttp = pathHttp + add;
            }

            //Create a filestream to the wanted path and copy the contents of the input stream to this 
            using (var fileStream = System.IO.File.Create(pathLocal))
            {
                file.CopyTo(fileStream);
            }

            SqlCommand command = new SqlCommand("INSERT INTO StagedFile(path) VALUES('" + pathHttp + "')", connection);

            if (command.ExecuteNonQuery() > 0)
            {
                command.CommandText = "SELECT IDENT_CURRENT('StagedFile')";
                return Int32.Parse(command.ExecuteScalar().ToString());
            }
            return -1;
        }

    }
}
