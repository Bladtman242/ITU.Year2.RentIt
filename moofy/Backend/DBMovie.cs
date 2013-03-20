using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace moofy.Backend
{
    class DBMovie
    {
        private static SqlConnection connection = new SqlConnection("user id=RentIt25db;" +
                                       "password=ZAQ12wsx;server=rentit.itu.dk;" +
                                       "Trusted_Connection=no;" +
                                       "database=RENTIT25; " +
                                       "connection timeout=30");

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
                    if (DBUser.Deposit(-price, userId))
                    {
                        command.CommandText = "INSERT INTO UserFile" +
                                              "VALUES(" + userId +
                                              ", " + movieId + ")";
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
    }
}
