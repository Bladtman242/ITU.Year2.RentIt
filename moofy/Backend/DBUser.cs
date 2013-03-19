using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace moofy.Backend {
    public static class DBUser {
        private static SqlConnection connection = new SqlConnection("user id=RentIt25db;" +
                                       "password=ZAQ12wsx;server=rentit.itu.dk;" +
                                       "Trusted_Connection=no;" +
                                       "database=RENTIT25; " +
                                       "connection timeout=30");

        //public static int AddUser(User user) {
        //    try {
        //        connection.Open();
        //        SqlCommand command = new SqlCommand("INSERT INTO Userz VALUES ('" + 
        //                                                user.Username + "', '" + 
        //                                                user.Password + "', '" + 
        //                                                user.Name + "', '" + 
        //                                                user.Email + "', " + 
        //                                                user.Balance + ")",
        //                                                connection);
        //        SqlDataReader reader = command.ExecuteReader();
        //    } catch (Exception e) {
        //        Console.WriteLine(e.ToString());
        //    } finally {
        //        connection.Close();
        //    }
        //}

        public static User GetUser(int userId) {
            try {
                connection.Open();
                //Get the data tied directly to the user table
                SqlCommand command = new SqlCommand("SELECT * FROM Userz WHERE id=" + userId,
                                                         connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read()) {
                    //Add user data to the object which will be returned
                    return new User(userId) {
                        Name = reader["name"].ToString(),
                        Username = reader["userName"].ToString(),
                        Balance = (int)reader["balance"],
                        Email = reader["email"].ToString(),
                        Password = reader["password"].ToString()
                    };
                }
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            } finally {
                connection.Close();
            }
            //Return null if no user has the specified id
            return null;
        }

        public static bool getIsAdmin(int userId) {
            try {
                connection.Open();
                //Get information as to wether this is an admin and add this information
                SqlCommand command = new SqlCommand("SELECT * FROM admin WHERE id=" + userId,
                                                    connection);
                SqlDataReader reader = command.ExecuteReader();
                return reader.Read();
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            } finally {
                connection.Close();
            }
            return false;
        }

        public static IList<Purchase> getPurchases(int userId) {
            //Get information on which movies the user has purchased
            IList<Purchase> purchases = new List<Purchase>();
            try {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM UserFile WHERE uid=" + userId,
                                                     connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    int fileId = (int)reader["fid"];
                    SqlCommand filequery = new SqlCommand("SELECT * FROM Filez WHERE id =" + fileId,
                                                          connection);
                    SqlCommand moviequery = new SqlCommand("SELECT * FROM Movie WHERE id =" + fileId,
                                                          connection);
                    SqlCommand songquery = new SqlCommand("SELECT * FROM Song WHERE id =" + fileId,
                                                           connection);
                    SqlDataReader fr = filequery.ExecuteReader();
                    SqlDataReader mr = moviequery.ExecuteReader();
                    SqlDataReader sr = songquery.ExecuteReader();

                    if (fr.Read()) {
                        if (mr.Read()) {
                            Movie mov = new Movie(fileId) {
                                BuyPrice = (float)fr["buyPrice"],
                                Description = fr["description"].ToString(),
                                Title = fr["title"].ToString(),
                                Uri = fr["URI"].ToString(),
                                Year = (short)fr["year"],
                                RentPrice = (float)fr["rentPrice"],
                                Director = mr["director"].ToString()
                            };
                            purchases.Add(new Purchase(mov, (DateTime)reader["endTime"]));
                        } else if (sr.Read()) {
                            Song song = new Song(fileId) {
                                BuyPrice = (float)fr["buyPrice"],
                                Description = fr["description"].ToString(),
                                Title = fr["title"].ToString(),
                                Uri = fr["URI"].ToString(),
                                Year = (short)fr["year"],
                                RentPrice = (float)fr["rentPrice"],
                                Album = sr["album"].ToString(),
                                Artist = sr["artist"].ToString()
                            };
                            purchases.Add(new Purchase(song, (DateTime)reader["endTime"]));
                        }

                    }
                }
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            } finally {
                connection.Close();
            }
            return purchases;
        }
    }
}
