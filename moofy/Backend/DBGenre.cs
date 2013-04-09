using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace moofy.Backend
{
    public partial class DBAccess 
    {
        public bool AddGenre(int fileId, string genre)
        {
            SqlCommand command = new SqlCommand("IF('"+genre+"' NOT IN (SELECT name FROM Genre)) "+
                                                "INSERT INTO Genre(name) VALUES('"+genre+"') "+
                                                "INSERT INTO GenreFile VALUES((SELECT id FROM Genre WHERE name='"+genre+"'), "+fileId+")",
                                                connection);

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
