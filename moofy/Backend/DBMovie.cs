using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace moofy.Backend
{
    public static partial class DBAccess
    {
        /// <summary>
        /// Return the movie with a given id
        /// </summary>
        /// <param name="movieId">The id of the movie to get</param>
        /// <returns>The movie with the given id, or null if no such movie exists</returns>
        public static Movie getMovie(int movieId)
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Movie WHERE id =" + movieId, connection);
                SqlDataReader reader = command.ExecuteReader();

                if(reader.Read())
                {
                    string director = reader["director"].ToString();
                    command.CommandText = "SELECT * FROM File WHERE id =" + movieId;
                    reader = command.ExecuteReader();
                    reader.Read();
                    return new Movie(movieId)
                    {
                        Director = director,
                        RentPrice = (float)reader["rentPrice"],
                        BuyPrice = (float)reader["buyPrice"],
                        Uri = reader["URI"].ToString(),
                        Title = reader["title"].ToString(),
                        Description = reader["description"].ToString(),
                        Year = (short)reader["year"]
                    };
                 }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                connection.Close();
            }
            return null;
        }

        /// <summary>
        /// Attempts to purchase a movie for a user
        /// </summary>
        /// <param name="movieId">The movie to purchase</param>
        /// <param name="userId">The user purchasing the movie</param>
        /// <returns>true if the movie is bought, false if the movie could no be bought (f.ex. due to insufficient funds), null if an error occurred</returns>
        public static bool? PurchaseMovie(int movieId, int userId)
        {
            try
            {
                connection.Open();
                //Get the price of the movie
                SqlCommand command = new SqlCommand("SELECT buyPrice FROM File WHERE id =" + movieId, connection);
                int price = (int)command.ExecuteScalar();
                //Get the balance of the user
                command.CommandText = "SELECT balance FROM Userz WHERE id =" + userId;
                int balance = (int)command.ExecuteScalar();
                if (balance - price >= 0)
                {
                    //Withdraw the amount from the users balance and only continue if it is successful
                    if (Deposit(-price, userId))
                    {
                        command.CommandText = "INSERT INTO UserFile" +
                                              "VALUES(" + userId +
                                              ", " + movieId + 
                                              ", " + DateTime.MaxValue + ")";
                        return command.ExecuteNonQuery() > 0;
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                connection.Close();
            }
            return null;
        }
    

        /// <summary>
        /// Attempts to rent a movie for a user
        /// </summary>
        /// <param name="movieId">The movie to rent</param>
        /// <param name="userId">The user renting the movie</param>
        /// <returns>true if the movie is rented, false if the movie could no be rented (i.e. due to insufficient funds), null if an error occurred</returns>
        public static bool? RentMovie(int movieId, int userId)
        {
            try
            {
                connection.Open();
                //Get the rental price of the movie
                SqlCommand command = new SqlCommand("SELECT rentPrice FROM File WHERE id =" + movieId, connection);
                int price = (int)command.ExecuteScalar();
                //Get the balance of the user
                command.CommandText = "SELECT balance FROM Userz WHERE id =" + userId;
                int balance = (int)command.ExecuteScalar();
                if (balance - price >= 0)
                {
                    //Withdraw the amount from the users balance and only continue if it is successful
                    if (Deposit(-price, userId))
                    {
                        command.CommandText = "INSERT INTO UserFile" +
                                              "VALUES(" + userId +
                                              ", " + movieId +
                                              ", " + DateTime.Now.AddDays(3) + ")";
                        return command.ExecuteNonQuery() > 0;
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                connection.Close();
            }
            return null;
        }

        /// <summary>
        /// Deletes a movie record and the corresponding file record, requires admin authorisation
        /// </summary>
        /// <param name="movieId">The movie to delete</param>
        /// <param name="adminId">The admin who authorised the deletion</param>
        /// <returns>true if the movie/file record is deleted or false otherwise</returns>
        public static bool DeleteMovie(int movieId, int adminId)
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Admin WHERE id =" + adminId, connection);
                if (command.ExecuteScalar() != null)
                {
                    //Delete the movie record first as it has a reference to the file record.
                    command.CommandText = "DELETE FROM Movie WHERE id=" + movieId;
                    if (command.ExecuteNonQuery() > 0)
                    {
                        command.CommandText = "DELETE FROM File WHERE id=" + movieId;
                        return command.ExecuteNonQuery() > 0;
                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                connection.Close();
            }
            return false;
        }
        /// <summary>
        /// Creates a movie in the database
        /// </summary>
        /// <param name="managerId">Id of the manager who wants to create the movie</param>
        /// <param name="tmpId">The temporary id the movie got when uploaded to the server</param>
        /// <param name="title">The title of the movie</param>
        /// <param name="year">The release year of the movie</param>
        /// <param name="buyPrice">The price to buy the movie</param>
        /// <param name="rentPrice">The price to rent the movie</param>
        /// <param name="director">The director of the movie</param>
        /// <param name="genres">The genres the movie fit into</param>
        /// <param name="description">A description of the movie</param>
        /// <returns>The id the movie is granted in the database or -1 if the movie could not be added to the database</returns>
        public static int CreateMovie(int managerId, string tmpId, string title, short year, int buyPrice,
                                        int rentPrice, string director, string[] genres, string description)
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Admin WHERE id =" + managerId, connection);
                if (command.ExecuteScalar() != null)
                {
                    //Get the uri from the StagedFile table
                    command.CommandText = "SELECT path FROM StagedFile WHERE id =" + tmpId;
                    string uri = command.ExecuteScalar().ToString();

                    //Add the file information into to Filez table
                    command.CommandText = "INSERT INTO Filez" +
                                          "(title, rentPrice, buyPrice, URI, year, description) " +
                                          "VALUES('" +
                                          title + "', " +
                                          rentPrice + ", " +
                                          buyPrice + ", '" +
                                          uri + "', " +
                                          year + ", '" +
                                          description + "')";

                    //If the information is successfully added continue to add info to the Movie table and GenreFile table
                    if (command.ExecuteNonQuery() > 0)
                    {
                        command.CommandText = "SELECT IDENT_CURRENT('Userz')";
                        int fileId = Int32.Parse(command.ExecuteScalar().ToString());

                        command.CommandText = "INSERT INTO Movie VALUES(" + fileId + ", '" + director + "')";
                        command.ExecuteNonQuery();

                        //Add genres to the file if any exist
                        if (genres.Length > 0)
                        {
                            string sql = "SELECT id FROM Genre WHERE ";
                            foreach (string genre in genres)
                            {
                                sql = sql + "name='" + genre + "' ";
                            }
                            command.CommandText = sql;
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                command.CommandText = "INSERT INTO GenreFile VALUES(" + reader["id"] + " ," + fileId + ")";
                                command.ExecuteNonQuery();
                            }

                        }
                        return fileId;
                    }
                }
           
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                connection.Close();
            }
            return -1;

        }
        /// <summary>
        /// Finds all movies where a parameter matches the filter given
        /// </summary>
        /// <param name="filter">The filter to match against</param>
        /// <returns>An array with all movies matching the filter or an array of size 0 if no movies match</returns>
        public static Movie[] FilterMovies(string filter)
        {
            try
            {
                connection.Open();
                List<Movie> movies = new List<Movie>();
                List<int> ids = new List<int>();//Ids of all movies added to the return list, used to prevent duplicates later

                //First get all rows from the movie table where an attribute matches the filter
                SqlCommand command = new SqlCommand("SELECT * FROM Movie "+
                                                    "WHERE director LIKE '%"+ filter+"%'"
                                                    , connection);
                SqlDataReader reader = command.ExecuteReader();

                //Get the rest of the info needed from the file table.
                while (reader.Read())
                {
                    int movieId = (int)reader["id"];
                    string director = reader["director"].ToString();
                    command.CommandText = "SELECT * FROM File WHERE id =" + movieId;
                    reader = command.ExecuteReader();
                    reader.Read();
                    movies.Add(new Movie(movieId)
                    {
                        Director = director,
                        RentPrice = (float)reader["rentPrice"],
                        BuyPrice = (float)reader["buyPrice"],
                        Uri = reader["URI"].ToString(),
                        Title = reader["title"].ToString(),
                        Description = reader["description"].ToString(),
                        Year = (short)reader["year"]
                    });
                    ids.Add(movieId);
                }

                //Now do the same for attributes in the filez table title
                // TILFØJ SØGNING PÅ GENRE ? :S
                command.CommandText ="SELECT * FROM Filez " +
                                     "WHERE title LIKE '%" + filter + "%' "+
                                     "OR description LIKE '%" + filter + "%' "+
                                     "OR id IN ("+
                                            "SELECT fid FROM GenreFile "+
                                            "WHERE gid =("+
                                                         "SELECT id FROM Genre "+
                                                         "WHERE name Like '%"+filter+"%'))";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = (int)reader["id"];
                    command.CommandText = "SELECT * FROM Movie WHERE id=" + id;
                    //Check if the file is a movie and that it has not already been added to the return set
                    SqlDataReader reader2 = command.ExecuteReader();
                    if (reader2.Read() && !ids.Contains(id))
                    {
                        movies.Add(new Movie(id)
                        {
                            Director = reader2["director"].ToString(),
                            RentPrice = (float)reader["rentPrice"],
                            BuyPrice = (float)reader["buyPrice"],
                            Uri = reader["URI"].ToString(),
                            Title = reader["title"].ToString(),
                            Description = reader["description"].ToString(),
                            Year = (short)reader["year"]
                        });
                    }
                }
                return movies.ToArray();
                        

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                connection.Close();
            }
            return new Movie[0];
        }


    }
}
