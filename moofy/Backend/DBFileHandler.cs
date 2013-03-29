﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.IO;

namespace moofy.Backend {
    public partial class DBAccess {

        /// <summary>
        /// Gets all genres for a file
        /// </summary>
        /// <param name="fileId">id of the file to get</param>
        /// <returns>a list of all the genre names</returns>
        public IList<string> GetGenres(int fileId)
        {
            SqlCommand command = new SqlCommand("SELECT name FROM Genre, GenreFile "+
                                                "WHERE GenreFile.fid =" + fileId +" "+
                                                "AND GenreFile.gid = Genre.id"
                                                , connection);

            SqlDataReader reader = command.ExecuteReader();
            IList<string> genres = new List<string>();
            while (reader.Read())
            {
                genres.Add(reader["name"].ToString());
            }
            
            return genres;
        }
        /// <summary>
        /// Returns the URI of the file with a given id
        /// </summary>
        /// <param name="fileId">The id of the file to download</param>
        /// <returns>The uri of the file or the empty string if the file does not exist in the database, or null if the user does not have access to download this movie</returns>
        public string DownloadFile(int fileId, int userId)
        {
            SqlCommand command = new SqlCommand("SELECT fid FROM UserFile WHERE fid="+fileId+" AND uid="+userId , connection);
            if (command.ExecuteScalar() == null) return null;

            command.CommandText = "SELECT uri FROM Filez WHERE id =" + fileId;
            Object uri = command.ExecuteScalar();

            if (uri == null) return "";//No file with the given id exists in the database
            else return uri.ToString();
        }

        /// <summary>
        /// Uploads a file to disk
        /// </summary>
        /// <param name="file">A stream with the info to be put in the file</param>
        /// <returns>The id of the tmp file added in the database</returns>
        public int UploadFile(Stream file) {
            string path = "C:\\RENTIT25\\" + file.GetHashCode();
            Random rand = new Random();

            //Ensure that no other file exists with this name
            while (new FileInfo(path).Exists)
            {
                path = path + rand.Next();
            }

            //Create a filestream to the wanted path and copy the contents of the input stream to this 
            using (var fileStream = System.IO.File.Create(path))
            {
                file.CopyTo(fileStream);
            }

            SqlCommand command = new SqlCommand("INSERT INTO StagedFile(path) VALUES('" + path + "')", connection);

            if (command.ExecuteNonQuery() > 0)
            {
                command.CommandText = "SELECT IDENT_CURRENT('StagedFile')";
                return Int32.Parse(command.ExecuteScalar().ToString());
            }
            return -1;
        }

    }
}
