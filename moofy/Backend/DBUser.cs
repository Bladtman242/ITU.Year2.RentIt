using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace moofy.Backend {
    public partial class DBAccess {

        public bool deleteUser(int userId) {
            SqlCommand command = new SqlCommand("DELETE FROM Userz WHERE id =" + userId, connection);
            return command.ExecuteNonQuery() > 0;
        }
        /// <summary>
        /// Demote a current admin to be just a regular user again
        /// </summary>
        /// <param name="demoterId">The ID of an who agrees to this demotion</param>
        /// <param name="demoteeId">The admin which will be demoted</param>
        /// <returns>success flag</returns>
        public bool DemoteAdmin(int demoterId, int demoteeId) {
            SqlCommand command = new SqlCommand("SELECT * FROM Admin WHERE id =" + demoterId, connection);
            if (command.ExecuteScalar() != null) {
                command.CommandText = "DELETE FROM Admin WHERE id=" + demoteeId;
                return command.ExecuteNonQuery() > 0;
            }
            return false;
        }

        /// <summary>
        /// Promoter a user to admin status
        /// </summary>
        /// <param name="promoterId">The ID of an admin who accepts this user as an admin</param>
        /// <param name="promoteeId">The user to be promoted</param>
        /// <returns>succes flag</returns>
        public bool PromotetoAdmin(int promoterId, int promoteeId) {
            SqlCommand command = new SqlCommand("SELECT * FROM Admin WHERE id =" + promoterId, connection);
            if (command.ExecuteScalar() != null) {
                command.CommandText = "INSERT INTO Admin VALUES(" + promoteeId + ")";
                return command.ExecuteNonQuery() > 0;
            }
            return false;
        }

        /// <summary>
        /// Deposits and amount to a user
        /// </summary>
        /// <param name="amount">The amount to deposit</param>
        /// <param name="userId">The Id of the user who will have the amount deposited</param>
        /// <returns>success flag</returns>
        public bool Deposit(int amount, int userId) {
            SqlCommand command = new SqlCommand("UPDATE Userz " +
                                                "SET balance = balance + " + amount +
                                                "WHERE id = " + userId,
                                                connection);
            return command.ExecuteNonQuery() > 0;
        }

        /// <summary>
        /// Checks if a user with a given username and password is valid
        /// </summary>
        /// <param name="uname">The username to check</param>
        /// <param name="password">The password to check</param>
        /// <returns>success flag</returns>
        public int Login(string uname, string password) {
            SqlCommand command = new SqlCommand("SELECT id FROM Userz " +
                                                "WHERE password='" + password + "'" +
                                                " AND userName='" + uname + "'",
                                                connection);
            Object id = command.ExecuteScalar();
            if (id != null) return (int)id;
            return -1;
        }

        /// <summary>
        /// Add a new user to the database
        /// </summary>
        /// <param name="user">The user to add</param>
        /// <returns>the id of the user, or -1 if the user could not be added</returns>
        public int AddUser(User user) {
            SqlCommand command = new SqlCommand("INSERT INTO Userz(userName, password, name, email, balance)" +
                                                    "VALUES ('" +
                                                    user.Username + "', '" +
                                                    user.Password + "', '" +
                                                    user.Name + "', '" +
                                                    user.Email + "', " +
                                                    user.Balance + ")",
                                                    connection);
            if (command.ExecuteNonQuery() > 0) {
                command.CommandText = "SELECT IDENT_CURRENT('Userz')";
                return Int32.Parse(command.ExecuteScalar().ToString());
            }
            return -1;
        }

        /// <summary>
        /// Returns a user from the database
        /// </summary>
        /// <param name="userId">The id of the user to return</param>
        /// <returns>The user with the given id or null if no such user exists</returns>
        public User GetUser(int userId) {
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
            reader.Close();
            //Return null if no user has the specified id
            return null;
        }

        /// <summary>
        /// Returns a flag indicating wether a user is admin
        /// </summary>
        /// <param name="userId">The id of the user to check if is admin</param>
        /// <returns>true if the user is admin else false</returns>
        public bool getIsAdmin(int userId) {
            //Get information as to wether this is an admin and add this information
            SqlCommand command = new SqlCommand("SELECT * FROM admin WHERE id=" + userId,
                                                connection);
            SqlDataReader reader = command.ExecuteReader();
            bool exist = reader.Read();
            reader.Close();
            return exist;
        }

        /// <summary>
        /// Returns all files purchased by a given user 
        /// </summary>
        /// <param name="userId">The id of the user to return all purchased files for</param>
        /// <returns>All files purchased by the user</returns>
        public IList<Purchase> getPurchases(int userId) {
            //Get information on which movies the user has purchased
            IList<Purchase> purchases = new List<Purchase>();
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
            reader.Close();
            return purchases;
        }
    }
}
