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
        /// Get all directors for a given movie
        /// </summary>
        /// <param name="movieId">The id of the movie</param>
        /// <returns>A list of directors for this movie</returns>
        public IList<string> GetDirectors(int movieId)
        {
            SqlCommand command = new SqlCommand("SELECT name FROM Director, MovieDirector " +
                                                "WHERE MovieDirector.moid =" + movieId + " " +
                                                "AND MovieDirector.did = Director.id"
                                                , connection);

            SqlDataReader reader = command.ExecuteReader();
            IList<string> directors = new List<string>();
            while (reader.Read())
            {
                directors.Add(reader["name"].ToString());
            }

            return directors;
        }
        /// <summary>
        /// Adds a director to a movie - if the director does not exist in the database he is added
        /// </summary>
        /// <param name="movieId">The id of the movie</param>
        /// <param name="director">The name of the director</param>
        /// <returns>a bool to indicate wether the transaction was successful</returns>
        public bool AddDirector(int movieId, string director)
        {
            SqlCommand command = new SqlCommand("SELECT id FROM Movie WHERE id =" + movieId, connection);
            if (command.ExecuteScalar() == null) return false;

            command.CommandText = "IF('" + director + "' NOT IN (SELECT name FROM Director)) " +
                                  "INSERT INTO Director(name) VALUES('" + director + "') " +
                                  "INSERT INTO MovieDirector VALUES(" + movieId +" ,(SELECT id FROM Director WHERE name='" + director + "'))";

            return command.ExecuteNonQuery() > 0;
        }
        /// <summary>
        /// Adds multiple directors to a movie - if a director does not exist in the database he is added
        /// </summary>
        /// <param name="movieId">The id of the movie</param>
        /// <param name="directors">names of the directors</param>
        /// <returns>a bool which indicates wether the transaction was a success</returns>
        public bool AddAllDirectors(int movieId, IEnumerable<string> directors)
        {
            bool succes = true;
            foreach (string director in directors)
            {
                if (!AddDirector(movieId, director))
                {
                    succes = false;
                    break;
                }
            }
            return succes;
        }
        /// <summary>
        /// Deletes all links to directors for a movie
        /// </summary>
        /// <param name="movieId">The movies id </param>
        /// <returns>A bool to indicate wether the deletion was a success</returns>
        public bool ClearMovieDirectors(int movieId)
        {
            SqlCommand command = new SqlCommand("DELETE FROM MovieDirector WHERE moid = " + movieId,
                                                connection);
            return command.ExecuteNonQuery() > 0;
        }
    }
}
