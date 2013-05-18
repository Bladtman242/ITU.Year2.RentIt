using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace moofy.Backend {
    public partial class DBAccess {
        /// <summary>
        /// Updates a user to have specified information
        /// </summary>
        /// <param name="user">a user object with the new information</param>
        /// <returns>true if the update is sucessful, false otherwise</returns>
        public bool UpdateUser(User user)
        {
            SqlCommand command = new SqlCommand("UPDATE Users "+
                                                "SET name ='"+ user.Name + "', "+
                                                "password ='" + user.Password + "', " +
                                                "email = '" + user.Email + "' WHERE id=" + user.Id,
                                                connection);

            return command.ExecuteNonQuery() > 0;
        }
        
        public bool DeleteUser(int userId) {

            SqlCommand command = new SqlCommand("DELETE FROM UserFile WHERE uid =" + userId +
                                                "DELETE FROM UserFileRating WHERE uid=" + userId
                                                , connection);
            command.ExecuteNonQuery();

            command.CommandText = "DELETE FROM Users WHERE id=" + userId;
            return command.ExecuteNonQuery() > 0;
        }
        /// <summary>
        /// Adds a rating to a file 
        /// </summary>
        /// <param name="userId">The user who rates the file</param>
        /// <param name="fileId">The file being rated</param>
        /// <param name="rating">The rating</param>
        /// <returns>A bool indicating wether the rating was successfully added to the database</returns>
        public bool RateFile(int userId, int fileId, int rating)
        {
            if(GetUser(userId) == null || (GetMovie(fileId)==null && GetSong(fileId)==null)) {
                return false;
            }
            SqlCommand command;
            if (GetRating(userId, fileId) >= 0)
            {
                command = new SqlCommand("UPDATE UserFileRating "+
                                         "SET rating = "+rating+
                                         "WHERE uid = " +userId+
                                         " AND fid = " + fileId,
                                         connection);
            }
            else
            {
                command = new SqlCommand("INSERT INTO UserFileRating(fid, uid, rating) " +
                                         "VALUES(" +
                                          fileId + ", " +
                                          userId + ", " +
                                          rating + ")",
                                          connection);
            }
            
            return command.ExecuteNonQuery() > 0;
        }
        /// <summary>
        /// gets the rating a user has given a file
        /// </summary>
        /// <param name="userId">the users id</param>
        /// <param name="fileId">the files id</param>
        /// <returns>the rating given by this user to this file, or -1 if the user have not yet rated this movie</returns>
        public int GetRating(int userId, int fileId)
        {
            SqlCommand command = new SqlCommand("SELECT rating FROM UserFileRating " +
                                                "WHERE fid = " + fileId +
                                                " AND uid = " + userId,
                                                connection);
            Object result = command.ExecuteScalar();
            if (result != null)
                return int.Parse(result.ToString());

            return -1;
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
            if (GetUser(promoterId) == null || GetUser(promoteeId) == null) {
                return false;
            }
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
            SqlCommand command = new SqlCommand("UPDATE Users " +
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
            SqlCommand command = new SqlCommand("SELECT id FROM Users " +
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
        /// <returns>Returns the user including the id of the user, or null if the user could not be added</returns>
        public User AddUser(User user) {
            SqlCommand command = new SqlCommand("SELECT id FROM Users " +
                                                "WHERE userName = '" + user.Username+"'",
                                                connection);
            if (command.ExecuteScalar() != null) return null;

            command.CommandText = "INSERT INTO Users(userName, password, name, email, balance)" +
                                  "VALUES ('" +
                                  user.Username + "', '" +
                                  user.Password + "', '" +
                                  user.Name + "', '" +
                                  user.Email + "', " +
                                  user.Balance + ")";

            if (command.ExecuteNonQuery() > 0) {
                command.CommandText = "SELECT IDENT_CURRENT('Users')";
                user.Id = Int32.Parse(command.ExecuteScalar().ToString());
                return user;
            }
            return new User() { Id = -1 };
        }

        /// <summary>
        /// Returns a user from the database
        /// </summary>
        /// <param name="userId">The id of the user to return</param>
        /// <returns>The user with the given id or null if no such user exists</returns>
        public User GetUser(int userId) {
            //Get the data tied directly to the user table
            SqlCommand command = new SqlCommand("SELECT * FROM Users WHERE id=" + userId,
                                                     connection);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read()) {
                //Add user data to the object which will be returned
                return new User() {
                    Id = userId,
                    Name = reader["name"].ToString(),
                    Username = reader["userName"].ToString(),
                    Balance = Int32.Parse(reader["balance"].ToString()),
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
        public bool GetIsAdmin(int userId) {
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
        public IList<Purchase> GetPurchases(int userId) {
            //Get information on which movies the user has purchased
            IList<Purchase> purchases = new List<Purchase>();
            SqlCommand command = new SqlCommand("SELECT * FROM UserFile WHERE uid=" + userId,
                                                 connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                int fileId = Int32.Parse(reader["fid"].ToString());
                SqlCommand filequery = new SqlCommand("SELECT * FROM Files WHERE id =" + fileId,
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
                        Movie mov = new Movie() {
                            Id = fileId,
                            BuyPrice = int.Parse(fr["buyPrice"].ToString()),
                            Description = fr["description"].ToString(),
                            Title = fr["title"].ToString(),
                            Uri = fr["URI"].ToString(),
                            Year = short.Parse(fr["year"].ToString()),
                            RentPrice = int.Parse(fr["rentPrice"].ToString())
                        };
                        purchases.Add(new Purchase(mov, (DateTime)reader["endTime"]));
                    } else if (sr.Read()) {
                        Song song = new Song() {
                            Id = fileId,
                            BuyPrice = int.Parse(fr["buyPrice"].ToString()),
                            Description = fr["description"].ToString(),
                            Title = fr["title"].ToString(),
                            Uri = fr["URI"].ToString(),
                            Year = short.Parse(fr["year"].ToString()),
                            RentPrice = int.Parse(fr["rentPrice"].ToString()),
                            Album = sr["album"].ToString()
                        };
                        purchases.Add(new Purchase(song, (DateTime.Parse(reader["endTime"].ToString()))));
                    }

                }
            }
            reader.Close();
            return purchases;
        }

        /// <summary>
        /// Returns all songs purchased by a given user 
        /// </summary>
        /// <param name="userId">The id of the user to return all purchased songs for</param>
        /// <returns>All songs purchased by the user</returns>
        public IList<Purchase> GetSongs(int userId) {
            //Get information on which songs the user has purchased
            IList<Purchase> purchases = new List<Purchase>();
            SqlCommand command = new SqlCommand("SELECT * FROM UserFile WHERE uid=" + userId,
                                                 connection);
            using (SqlDataReader reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    int fileId = Int32.Parse(reader["fid"].ToString());
                    SqlCommand filequery = new SqlCommand("SELECT * FROM Files WHERE id =" + fileId,
                                                          connection);
                    SqlCommand songquery = new SqlCommand("SELECT * FROM Song WHERE id =" + fileId,
                                                       connection);
                    SqlDataReader fr = filequery.ExecuteReader();
                    SqlDataReader sr = songquery.ExecuteReader();

                    if (fr.Read()) {
                        if (sr.Read()) {
                            Song song = new Song() {
                                Id = fileId,
                                BuyPrice = int.Parse(fr["buyPrice"].ToString()),
                                Description = fr["description"].ToString(),
                                Title = fr["title"].ToString(),
                                Uri = fr["URI"].ToString(),
                                Year = short.Parse(fr["year"].ToString()),
                                RentPrice = int.Parse(fr["rentPrice"].ToString()),
                                Album = sr["album"].ToString()
                            };
                            purchases.Add(new Purchase(song, (DateTime.Parse(reader["endTime"].ToString()))));
                        }
                    }
                }
            }
            return purchases;
        }




        /// <summary>
        /// Returns all movies purchased by a given user 
        /// </summary>
        /// <param name="userId">The id of the user to return all purchased movies for</param>
        /// <returns>All movies purchased by the user</returns>
        public IList<Purchase> GetMovies(int userId) {
            //Get information on which movies the user has purchased
            IList<Purchase> purchases = new List<Purchase>();
            SqlCommand command = new SqlCommand("SELECT * FROM UserFile WHERE uid=" + userId,
                                                 connection);
            using (SqlDataReader reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    int fileId = Int32.Parse(reader["fid"].ToString());
                    SqlCommand filequery = new SqlCommand("SELECT * FROM Files WHERE id =" + fileId,
                                                          connection);
                    SqlCommand moviequery = new SqlCommand("SELECT * FROM Movie WHERE id =" + fileId,
                                                          connection);
                    SqlDataReader fr = filequery.ExecuteReader();
                    SqlDataReader mr = moviequery.ExecuteReader();

                    if (fr.Read()) {
                        if (mr.Read()) {
                            Console.WriteLine(fr["description"].ToString());
                            Movie mov = new Movie() {
                                Id = fileId,
                                BuyPrice = int.Parse(fr["buyPrice"].ToString()),
                                Description = fr["description"].ToString(),
                                Title = fr["title"].ToString(),
                                Uri = fr["URI"].ToString(),
                                Year = short.Parse(fr["year"].ToString()),
                                RentPrice = int.Parse(fr["rentPrice"].ToString())
                            };
                            purchases.Add(new Purchase(mov, (DateTime)reader["endTime"]));
                        }
                    }
                }
            }
            Console.WriteLine(purchases.Count);
            Console.WriteLine(purchases.Count);
            return purchases;
        }
    }
}
