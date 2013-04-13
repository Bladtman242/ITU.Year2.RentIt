using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace moofy.Backend {
    public partial class DBAccess {

        /// <summary>
        /// Updates a movie to contain the given values 
        /// </summary>
        /// <param name="movie">A movie object which contains the new values of the movie</param>
        /// <param name="adminId">The id of the admin who authorises </param>
        /// <returns>True if the update is sucessful, false otherwise </returns>
        public bool UpdateMovie(Movie movie, int adminId)
        {
            //Ensure that the admin id does infact belong to an admin
            SqlCommand command = new SqlCommand("SELECT id FROM Admin WHERE id =" + adminId, connection);
            if (command.ExecuteScalar() == null) return false;

            command.CommandText = "UPDATE Filez " +
                                  "SET title = '" + movie.Title.Replace("'", "''") + "', " +
                                  "description = '" + movie.Description.Replace("'", "''") + "', " +
                                  "rentPrice = " + movie.RentPrice + ", " +
                                  "buyPrice = " + movie.BuyPrice + ", " +
                                  "year = " + movie.Year + ", " +
                                  "coverURI = '" + movie.CoverUri.Replace("'", "''") + "' " +
                                  "WHERE id =" + movie.Id +
                                  " UPDATE Movie SET director = '" + movie.Director.Replace("'", "''") + "' " +
                                  "WHERE id = " + movie.Id;

            return command.ExecuteNonQuery() > 0;
        }
        /// <summary>
        /// Return the movie with a given id
        /// </summary>
        /// <param name="movieId">The id of the movie to get</param>
        /// <returns>The movie with the given id, or null if no such movie exists</returns>
        public Movie GetMovie(int movieId) {
            SqlCommand command = new SqlCommand("SELECT * FROM Movie WHERE id =" + movieId, connection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read()) {
                string director = reader["director"].ToString();
                reader.Close();
                command.CommandText = "SELECT * FROM Filez WHERE id =" + movieId;
                reader = command.ExecuteReader();
                reader.Read();
                Movie mov = new Movie() {
                    Id = movieId,
                    Director = director,
                    RentPrice = int.Parse(reader["rentPrice"].ToString()),
                    BuyPrice = int.Parse(reader["buyPrice"].ToString()),
                    Uri = reader["URI"].ToString(),
                    Title = reader["title"].ToString(),
                    Description = reader["description"].ToString(),
                    Year = short.Parse(reader["year"].ToString()),
                    CoverUri = reader["coverURI"].ToString(),
                    ViewCount = int.Parse(reader["viewCount"].ToString())
                };
                reader.Close();
                return mov;

            }
            reader.Close();
            return null;
        }

        /// <summary>
        /// Attempts to purchase a movie for a user
        /// </summary>
        /// <param name="movieId">The movie to purchase</param>
        /// <param name="userId">The user purchasing the movie</param>
        /// <returns>true if the movie is bought, false if the movie could not be bought (f.ex. due to insufficient funds)</returns>
        public bool PurchaseMovie(int movieId, int userId) {
            //Ensure that the id is valid for a movie
            SqlCommand command = new SqlCommand("SELECT id FROM Movie WHERE id=" + movieId, connection);
            if (command.ExecuteScalar() == null) return false;

            //Get the price of the movie
            command.CommandText = "SELECT buyPrice FROM Filez WHERE id =" + movieId;
            Object pric = command.ExecuteScalar();
            if (pric == null) return false;
            int price = (int)pric;
            //Get the balance of the user
            command.CommandText = "SELECT balance FROM Userz WHERE id =" + userId;
            Object bal = command.ExecuteScalar();
            if (bal == null) return false;
            int balance = (int)bal;
            if (balance - price >= 0) {
                //Withdraw the amount from the users balance and only continue if it is successful
                command.CommandText = "UPDATE Userz " +
                                      "SET balance = balance - " + price +
                                      "WHERE id = " + userId;
                if (command.ExecuteNonQuery() > 0) {
                    command.CommandText = "INSERT INTO UserFile " +
                                          "VALUES(" + userId +
                                          ", " + movieId +
                                          ", '" + DateTime.MaxValue.ToString("yyyy-MM-dd HH:mm:ss") + "' )";
                    return command.ExecuteNonQuery() > 0;
                }
            }
            return false;
        }


        /// <summary>
        /// Attempts to rent a movie for a user
        /// </summary>
        /// <param name="movieId">The movie to rent</param>
        /// <param name="userId">The user renting the movie</param>
        /// <returns>true if the movie is rented, false if the movie could no be rented (i.e. due to insufficient funds)</returns>
        public bool RentMovie(int movieId, int userId, int days) {
            //Ensure that the id is valid for a movie
            SqlCommand command = new SqlCommand("SELECT id FROM Movie WHERE id=" + movieId, connection);
            if (command.ExecuteScalar() == null) return false;

            //Get the rental price of the movie
            command.CommandText = "SELECT rentPrice FROM Filez WHERE id =" + movieId;
            Object pric = command.ExecuteScalar();
            if (pric == null) return false;
            int price = (int)pric;
            //Get the balance of the user
            command.CommandText = "SELECT balance FROM Userz WHERE id =" + userId;
            Object bal = command.ExecuteScalar();
            if (bal == null) return false;
            int balance = (int)bal;
            if (balance - price >= 0) {
                //Withdraw the amount from the users balance and only continue if it is successful
                command.CommandText = "UPDATE Userz " +
                                      "SET balance = balance - " + price +
                                      "WHERE id = " + userId;
                if (command.ExecuteNonQuery() > 0) {
                    command.CommandText = "INSERT INTO UserFile " +
                                          "VALUES(" + userId +
                                          ", " + movieId +
                                          ", '" + DateTime.Now.AddDays(days).ToString("yyyy-MM-dd HH:mm:ss") + "' )";
                    return command.ExecuteNonQuery() > 0;
                }
            }
            return false;
        }

        /// <summary>
        /// Deletes a movie record and the corresponding file record, requires admin authorisation
        /// </summary>
        /// <param name="movieId">The movie to delete</param>
        /// <param name="adminId">The admin who authorised the deletion</param>
        /// <returns>true if the movie/file record is deleted or false otherwise</returns>
        public bool DeleteMovie(int movieId, int adminId) {
            SqlCommand command = new SqlCommand("SELECT * FROM Admin WHERE id =" + adminId, connection);
            if (command.ExecuteScalar() != null) {
                //Delete the movie record first as it has a reference to the file record.
                command.CommandText = "DELETE FROM Movie WHERE id=" + movieId;
                if (command.ExecuteNonQuery() > 0) {
                    command.CommandText = "DELETE FROM GenreFile WHERE fid=" + movieId +
                                          " DELETE FROM UserFile WHERE fid=" + movieId +
                                          " DELETE FROM UserFileRating WHERE fid=" + movieId +
                                          " DELETE FROM Filez WHERE id=" + movieId;
                    
                    return command.ExecuteNonQuery() > 0;
                  
                }

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
        /// <returns>The movie including id and uri or null if the movie could not be added to the database</returns>
        public Movie CreateMovie(int managerId, int tmpId, IList<string> genres, Movie movie) {
            SqlCommand command = new SqlCommand("SELECT * FROM Admin WHERE id =" + managerId, connection);
            if (command.ExecuteScalar() != null) {
                //Get the uri from the StagedFile table
                command.CommandText = "SELECT path FROM StagedFile WHERE id =" + tmpId;
                object tmpUri = command.ExecuteScalar();
                if (tmpUri == null) return null;

                string uri = tmpUri.ToString();

                //Add the file information into to Filez table
                command.CommandText = "INSERT INTO Filez" +
                                      "(title, rentPrice, buyPrice, URI, year, description, coverURI, viewCount) " +
                                      "VALUES('" +
                                      movie.Title.Replace("'", "''") + "', " +
                                      movie.RentPrice + ", " +
                                      movie.BuyPrice + ", '" +
                                      uri.Replace("'", "''") + "', " +
                                      movie.Year + ", '" +
                                      movie.Description.Replace("'", "''") + "', '" +
                                      movie.CoverUri.Replace("'", "''") + "', " +
                                      "0)";

                //If the information is successfully added continue to add info to the Movie table and GenreFile table
                if (command.ExecuteNonQuery() > 0) {
                    command.CommandText = "SELECT IDENT_CURRENT('Filez')";
                    int fileId = Int32.Parse(command.ExecuteScalar().ToString());

                    command.CommandText = "INSERT INTO Movie VALUES(" + fileId + ", '" + movie.Director + "')";
                    command.ExecuteNonQuery();

                    //Add genres to the file if any exist
                    if (genres.Count > 0) {
                        AddAllGenres(fileId, genres);
                        //string sql = "SELECT id FROM Genre WHERE name IN ('" +
                        //              string.Join<string>("', '", genres) + "')";
                        //command.CommandText = sql;
                        //SqlDataReader reader = command.ExecuteReader();
                        //while (reader.Read()) {
                        //    SqlCommand command2 = new SqlCommand("INSERT INTO GenreFile VALUES(" + reader["id"] + " ," + fileId + ")", connection);
                        //    command2.ExecuteNonQuery();
                        //}
                        //reader.Close();

                    }
                    command.CommandText = "DELETE FROM StagedFile WHERE id=" + tmpId;
                    command.ExecuteNonQuery();
                    movie.Id = fileId;
                    movie.Uri = uri;
                    return movie;
                }
            }
            return null;

        }
        /// <summary>
        /// Finds all movies where a parameter matches the filter given
        /// </summary>
        /// <param name="filter">The filter to match against</param>
        /// <returns>An array with all movies matching the filter or an array of size 0 if no movies match</returns>
        public Movie[] FilterMovies(string filter) {
            List<Movie> movies = new List<Movie>();

            //First get all rows from the movie table where an attribute matches the filter
            SqlCommand command = new SqlCommand("SELECT * FROM Movie , Filez " +
                                                "WHERE Movie.id = Filez.id " +
                                                "AND (director LIKE '%" + filter + "%' " +
                                                "OR title LIKE '%" + filter + "%' " +
                                                "OR description LIKE '%" + filter + "%' " +
                                                "OR Filez.id IN (" +
                                                    "SELECT fid FROM GenreFile " +
                                                    "WHERE gid =(" +
                                                        "SELECT id FROM Genre " +
                                                        "WHERE name Like '%" + filter + "%' )))"
                                                , connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read()) {

                movies.Add(new Movie() {
                    Id = Int32.Parse(reader["id"].ToString()),
                    Director = reader["director"].ToString(),
                    RentPrice = int.Parse(reader["rentPrice"].ToString()),
                    BuyPrice = int.Parse(reader["buyPrice"].ToString()),
                    Uri = reader["URI"].ToString(),
                    Title = reader["title"].ToString(),
                    Description = reader["description"].ToString(),
                    Year = short.Parse(reader["year"].ToString()),
                    CoverUri = reader["coverURI"].ToString(),
                    ViewCount = int.Parse(reader["viewCount"].ToString())
                });
            }

            reader.Close();
            return movies.ToArray();
        }


    }
}
