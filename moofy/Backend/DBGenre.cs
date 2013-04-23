using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace moofy.Backend
{
    public partial class DBAccess 
    {
        /// <summary>
        /// Gets all genres for a file
        /// </summary>
        /// <param name="fileId">id of the file to get</param>
        /// <returns>a list of all the genre names</returns>
        public IList<string> GetGenres(int fileId)
        {
            SqlCommand command = new SqlCommand("SELECT name FROM Genre, GenreFile " +
                                                "WHERE GenreFile.fid =" + fileId + " " +
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
        public bool AddGenre(int fileId, string genre)
        {
            SqlCommand command = new SqlCommand("SELECT id FROM Files WHERE id =" + fileId, connection);
            if(command.ExecuteScalar() == null) return false;

            command.CommandText = "IF('" + genre + "' NOT IN (SELECT name FROM Genre)) " +
                                  "INSERT INTO Genre(name) VALUES('" + genre + "') " +
                                  "INSERT INTO GenreFile VALUES((SELECT id FROM Genre WHERE name='" + genre + "'), " + fileId + ")";

            return command.ExecuteNonQuery() > 0;
        }

        public bool AddAllGenres(int fileId, IEnumerable<string> genres)
        {
            bool succes = true;
            foreach (string genre in genres)
            {
                if (!AddGenre(fileId, genre))
                {
                    succes = false;
                    break;
                }
            }
            return succes;
        }

        public bool ClearFileGenres(int fileId)
        {
            SqlCommand command = new SqlCommand("DELETE FROM GenreFile WHERE fid = " + fileId,
                                                connection);
            return command.ExecuteNonQuery() > 0;
        }

    }
}
