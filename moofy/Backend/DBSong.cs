using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace moofy.Backend {
    partial class DBAccess {
        /// <summary>
        /// Updates a song to contain new values
        /// </summary>
        /// <param name="song">The song object containing the new information</param>
        /// <returns>true if the new values are succesfully added, false otherwise.</returns>
        public bool UpdateSong(Song song, int adminId) {
            //Ensure that the admin id does infact belong to an admin
            SqlCommand command = new SqlCommand("SELECT id FROM Admin WHERE id =" + adminId, connection);
            if (command.ExecuteScalar() == null) return false;

            command.CommandText =   ("UPDATE Files " +
                                     "SET title = '" + song.Title.Replace("'", "''") + "', " +
                                     "description = '" + song.Description.Replace("'", "''") + "', " +
                                     "rentPrice = " + song.RentPrice + ", " +
                                     "buyPrice = " + song.BuyPrice + ", " +
                                     "year = " + song.Year + ", " +
                                     "coverURI = '" + song.CoverUri.Replace("'", "''") + "'" +
                                     "WHERE id =" + song.Id +
                                     "UPDATE Song SET album = '" + song.Album.Replace("'", "''") + "', " +
                                     "artist = '" + song.Artist.Replace("'", "''") + "' " +
                                     "WHERE id = " + song.Id);

            return command.ExecuteNonQuery() > 0;
        }

        /// <summary>
        /// Return the song with a given id
        /// </summary>
        /// <param name="songId">The id of the song to get</param>
        /// <returns>The song with the given id, or null if no such movie exists</returns>
        public Song GetSong(int songId) {
            SqlCommand command = new SqlCommand("SELECT * FROM Song WHERE id =" + songId, connection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read()) {
                string artist = reader["artist"].ToString();
                string album = reader["album"].ToString();
                reader.Close();
                command.CommandText = "SELECT * FROM Files WHERE id =" + songId;
                reader = command.ExecuteReader();
                reader.Read();
                Song song = new Song() {
                    Id = songId,
                    Album = album,
                    Artist = artist,
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
                return song;

            }
            reader.Close();
            return null;
        }

        /// <summary>
        /// Attempts to purchase a song for a user
        /// </summary>
        /// <param name="songId">The movie to purchase</param>
        /// <param name="userId">The user purchasing the movie</param>
        /// <returns>true if the song is bought, false if the song could not be bought (f.ex. due to insufficient funds)</returns>
        public bool PurchaseSong(int songId, int userId) {
            //Ensure that the id identifies a song
            SqlCommand command = new SqlCommand("SELECT id FROM Song WHERE id=" + songId, connection);
            if (command.ExecuteScalar() == null) return false;

            //Get the price of the song
            command.CommandText = "SELECT buyPrice FROM Files WHERE id =" + songId;
            Object pric = command.ExecuteScalar();
            if (pric == null) return false;
            int price = (int)pric;
            //Get the balance of the user
            command.CommandText = "SELECT balance FROM Users WHERE id =" + userId;
            Object bal = command.ExecuteScalar();
            if (bal == null) return false;
            int balance = (int)bal;
            if (balance - price >= 0) {
                //Withdraw the amount from the users balance and only continue if it is successful
                command.CommandText = "UPDATE Users " +
                                      "SET balance = balance - " + price +
                                      "WHERE id = " + userId;
                if (command.ExecuteNonQuery() > 0) {
                    command.CommandText = "INSERT INTO UserFile " +
                                          "VALUES(" + userId +
                                          ", " + songId +
                                          ", '" + DateTime.MaxValue.ToString("yyyy-MM-dd HH:mm:ss") + "' )";
                    return command.ExecuteNonQuery() > 0;
                }
            }
            return false;
        }

        /// <summary>
        /// Attempts to purchase a song for a user
        /// </summary>
        /// <param name="songId">The movie to purchase</param>
        /// <param name="userId">The user purchasing the movie</param>
        /// <returns>true if the song is bought, false if the song could not be bought (f.ex. due to insufficient funds)</returns>
        public bool RentSong(int songId, int userId, int days) {
            //Ensure that the id identifies a song
            SqlCommand command = new SqlCommand("SELECT id FROM Song WHERE id=" + songId, connection);
            if (command.ExecuteScalar() == null) return false;

            //Get the price of the song
            command.CommandText = "SELECT rentPrice FROM Files WHERE id =" + songId;
            Object pric = command.ExecuteScalar();
            if (pric == null) return false;
            int price = (int)pric;
            //Get the balance of the user
            command.CommandText = "SELECT balance FROM Users WHERE id =" + userId;
            Object bal = command.ExecuteScalar();
            if (bal == null) return false;
            int balance = (int)bal;
            if (balance - price >= 0) {
                //Withdraw the amount from the users balance and only continue if it is successful
                command.CommandText = "UPDATE Users " +
                                      "SET balance = balance - " + price +
                                      "WHERE id = " + userId;
                if (command.ExecuteNonQuery() > 0) {
                    command.CommandText = "INSERT INTO UserFile " +
                                          "VALUES(" + userId +
                                          ", " + songId +
                                          ", '" + DateTime.Now.AddDays(days).ToString("yyyy-MM-dd HH:mm:ss") + "' )";
                    return command.ExecuteNonQuery() > 0;
                }
            }
            return false;
        }

        /// <summary>
        /// Deletes a song record and the corresponding file record from the database, requires admin authorisation
        /// </summary>
        /// <param name="songId">The song to delete</param>
        /// <param name="adminId">The admin who authorises this deletion</param>
        /// <returns>true if the song/file record is deleted or false otherwise</returns>
        public bool DeleteSong(int songId, int adminId) {
            SqlCommand command = new SqlCommand("SELECT * FROM Admin WHERE id =" + adminId, connection);
            if (command.ExecuteScalar() != null) {
                //Delete the movie record first as it has a reference to the file record.
                command.CommandText = "DELETE FROM Song WHERE id=" + songId;
                if (command.ExecuteNonQuery() > 0) {
                    command.CommandText = "DELETE FROM GenreFile WHERE fid=" + songId +
                                          " DELETE FROM UserFileRating WHERE fid=" + songId+
                                          " DELETE FROM UserFile WHERE fid=" + songId +
                                          " DELETE FROM Files WHERE id=" + songId;
                    return command.ExecuteNonQuery() > 0;
                  
                }

            }
            return false;
        }

        /// <summary>
        /// Creates a database record for a song which has been uploaded to the server previously
        /// </summary>
        /// <param name="managerId">the id of the manager who creates the song record</param>
        /// <param name="tmpId">the tmp id the song file has in the system</param>
        /// <param name="title">the title of the song</param>
        /// <param name="year">The year the song was released</param>
        /// <param name="buyPrice">The buy price of the song</param>
        /// <param name="rentPrice">The rent price of the song</param>
        /// <param name="album">The album the song is featured on</param>
        /// <param name="artist">The artist who made the song</param>
        /// <param name="genres">The genres the song fits into</param>
        /// <param name="description">The description of the song</param>
        /// <returns>The Song object including id and uri , or null if the song could not be added to the database</returns>
        public Song CreateSong(int managerId, int tmpId, IList<String> genres, Song song) {
            SqlCommand command = new SqlCommand("SELECT * FROM Admin WHERE id =" + managerId, connection);
            if (command.ExecuteScalar() != null) {
                //Get the uri from the StagedFile table
                command.CommandText = "SELECT path FROM StagedFile WHERE id =" + tmpId;
                object tmpUri = command.ExecuteScalar();
                if (tmpUri == null) return null;
                string uri = tmpUri.ToString();
                //Add the file information into to Files table
                command.CommandText = "INSERT INTO Files" +
                                      "(title, rentPrice, buyPrice, URI, year, description, coverURI, viewCount) " +
                                      "VALUES('" +
                                      song.Title.Replace("'", "''") + "', " +
                                      song.RentPrice + ", " +
                                      song.BuyPrice + ", '" +
                                      uri.Replace("'", "''") + "', " +
                                      song.Year + ", '" +
                                      song.Description.Replace("'", "''") + "', '" +
                                      song.CoverUri.Replace("'", "''") + "', " +
                                      "0)";

                //If the information is successfully added continue to add info to the Movie table and GenreFile table
                if (command.ExecuteNonQuery() > 0) {
                    command.CommandText = "SELECT IDENT_CURRENT('Files')";
                    int fileId = Int32.Parse(command.ExecuteScalar().ToString());

                    command.CommandText = "INSERT INTO Song VALUES(" + fileId + ", '" + song.Artist + "', '" + song.Album + "')";
                    command.ExecuteNonQuery();

                    //Add genres to the file if any exist
                    if (genres.Count() > 0) {
                        string sql = "SELECT id FROM Genre WHERE name IN ('" +
                                      string.Join<string>("', '", genres) + "')";
                        command.CommandText = sql;
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read()) {
                            SqlCommand command2 = new SqlCommand("INSERT INTO GenreFile VALUES(" + reader["id"] + " ," + fileId + ")", connection);
                            command2.ExecuteNonQuery();
                        }
                        reader.Close();

                    }
                    command.CommandText = "DELETE FROM StagedFile WHERE id=" + tmpId;
                    command.ExecuteNonQuery();
                    song.Uri = uri;
                    song.Id = fileId;
                    return song;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets all songs which matches the filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Song[] FilterSongs(string filter) {
            List<Song> songs = new List<Song>();

            //First get all rows from the song and file table joined, where an attribute matches the filter
            SqlCommand command = new SqlCommand("SELECT * FROM Song , Files " +
                                                "WHERE Song.id = Files.id " +
                                                "AND (artist LIKE '%" + filter + "%' " +
                                                "OR album LIKE '%" + filter + "%' " +
                                                "OR title LIKE '%" + filter + "%' " +
                                                "OR description LIKE '%" + filter + "%' " +
                                                "OR Files.id IN (" +
                                                    "SELECT fid FROM GenreFile " +
                                                    "WHERE gid =(" +
                                                        "SELECT id FROM Genre " +
                                                        "WHERE name Like '%" + filter + "%' )))"
                                                , connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read()) {

                songs.Add(new Song() {
                    Id = Int32.Parse(reader["id"].ToString()),
                    Artist = reader["artist"].ToString(),
                    Album = reader["album"].ToString(),
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
            return songs.ToArray();
        }

    }
}
