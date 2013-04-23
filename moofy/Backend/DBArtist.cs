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
        /// Gets all artists for a song
        /// </summary>
        /// <param name="songId">id of the song</param>
        /// <returns>a list of all the artist names</returns>
        public IList<string> GetArtists(int songId)
        {
            SqlCommand command = new SqlCommand("SELECT name FROM Artist, SongArtist " +
                                                "WHERE SongArtist.sid =" + songId + " " +
                                                "AND SongArtist.aid = Artist.id"
                                                , connection);

            SqlDataReader reader = command.ExecuteReader();
            IList<string> artists = new List<string>();
            while (reader.Read())
            {
                artists.Add(reader["name"].ToString());
            }

            return artists;
        }
        public bool AddArtist(int songId, string artist)
        {
            SqlCommand command = new SqlCommand("SELECT id FROM Song WHERE id =" + songId, connection);
            if (command.ExecuteScalar() == null) return false;

            command.CommandText = "IF('" + artist + "' NOT IN (SELECT name FROM Artist)) " +
                                  "INSERT INTO Artist(name) VALUES('" + artist + "') " +
                                  "INSERT INTO SongArtist VALUES((SELECT id FROM Artist WHERE name='" + artist + "'), " + songId + ")";

            return command.ExecuteNonQuery() > 0;
        }

        public bool AddAllArtists(int songId, IEnumerable<string> artists)
        {
            bool succes = true;
            foreach (string artist in artists)
            {
                if (!AddArtist(songId, artist))
                {
                    succes = false;
                    break;
                }
            }
            return succes;
        }

        public bool ClearSongArtists(int songId)
        {
            SqlCommand command = new SqlCommand("DELETE FROM SongArtist WHERE sid = " + songId,
                                                connection);
            return command.ExecuteNonQuery() > 0;
        }
    }
}
