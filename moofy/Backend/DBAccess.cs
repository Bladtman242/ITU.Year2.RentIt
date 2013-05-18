using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace moofy.Backend {
    public partial class DBAccess {
        protected SqlConnection connection;
       
        public DBAccess() {
            connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]);//Skal evt. ændres til WebConfigurationManager.AppSettings for at virke med webservicen deployed på rentit.itu.dk
        }

        public void Open() {
            connection.Open();
        }

        public void Close() {
            connection.Close();
        }
        /// <summary>
        /// FOR TESTING ONLY, WILL CLEAR THE DATABASE AND ADD GERNERIC TEST INFORMATION
        /// </summary>
        public void ClearDatabase()
        {
            SqlCommand command = new SqlCommand("IF OBJECT_ID('StagedFile') IS NOT NULL DROP TABLE StagedFile " +
                                                "IF OBJECT_ID('MovieDirector') IS NOT NULL DROP TABLE MovieDirector "+
                                                "IF OBJECT_ID('SongArtist') IS NOT NULL DROP TABLE SongArtist "+
                                                "IF OBJECT_ID('Movie') IS NOT NULL DROP TABLE Movie " +
                                                "IF OBJECT_ID('Song') IS NOT NULL DROP TABLE Song " +
                                                "IF OBJECT_ID('GenreFile') IS NOT NULL DROP TABLE GenreFile " +
                                                "IF OBJECT_ID('UserFile') IS NOT NULL DROP TABLE UserFile " +
                                                "IF OBJECT_ID('UserFileRating') IS NOT NULL DROP TABLE UserFileRating " +
                                                "IF OBJECT_ID('Files') IS NOT NULL DROP TABLE Files " +
                                                "IF OBJECT_ID('Genre') IS NOT NULL DROP TABLE Genre " +
                                                "IF OBJECT_ID('Admin') IS NOT NULL DROP TABLE Admin " +
                                                "IF OBJECT_ID('Users') IS NOT NULL DROP TABLE Users " +
                                                "IF OBJECT_ID('Director') IS NOT NULL DROP TABLE Director "+
                                                "IF OBJECT_ID('Artist') IS NOT NULL DROP TABLE Artist "+
                                                "CREATE TABLE StagedFile (id INT IDENTITY(1,1) PRIMARY KEY NOT NULL, path VARCHAR(200) NOT NULL) "+
                                                "CREATE TABLE Files (id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,title VARCHAR(200) NOT NULL,rentPrice INT NOT NULL, buyPrice INT NOT NULL, URI VARCHAR(200) NOT NULL, year INT NOT NULL, description VARCHAR(1000), coverURI VARCHAR(200) NOT NULL, viewCount INT NOT NULL) "+
                                                "CREATE TABLE Movie (id INT PRIMARY KEY NOT NULL, FOREIGN KEY (id) REFERENCES Files(id)) "+
                                                "CREATE TABLE Director (id INT IDENTITY(1,1) PRIMARY KEY NOT NULL, name VARCHAR(150)) "+
                                                "CREATE TABLE MovieDirector (moid INT NOT NULL, did INT NOT NULL, FOREIGN KEY (moid) REFERENCES Movie(id), FOREIGN KEY (did) REFERENCES Director(id)) "+
                                                "CREATE TABLE Song (id INT PRIMARY KEY NOT NULL, album VARCHAR(150), FOREIGN KEY (id) REFERENCES Files(id)) "+
                                                "CREATE TABLE Artist (id INT IDENTITY(1,1) PRIMARY KEY NOT NULL, name VARCHAR(150)) "+
                                                "CREATE TABLE SongArtist (sid INT NOT NULL, aid INT NOT NULL, FOREIGN KEY (sid) REFERENCES Song(id), FOREIGN KEY (aid) REFERENCES Artist(id)) "+
                                                "CREATE TABLE Genre (id INT IDENTITY(1,1) PRIMARY KEY NOT NULL, name VARCHAR(150)) "+
                                                "CREATE TABLE GenreFile (gid INT NOT NULL, fid INT NOT NULL, PRIMARY KEY (gid, fid), FOREIGN KEY (gid) REFERENCES Genre(id), FOREIGN KEY (fid) REFERENCES Files(id)) "+
                                                "CREATE TABLE Users (id INT IDENTITY(1,1) PRIMARY KEY NOT NULL, userName VARCHAR(150) NOT NULL, password VARCHAR(150) NOT NULL, name VARCHAR(150), email VARCHAR(150) UNIQUE, balance INT NOT NULL) "+
                                                "CREATE TABLE UserFile (uid INT NOT NULL, fid INT NOT NULL, endTime DATETIME not null, PRIMARY KEY (uid, fid), FOREIGN KEY (uid) REFERENCES Users(id), FOREIGN KEY (fid) REFERENCES Files(id)) "+
                                                "CREATE TABLE UserFileRating (uid INT NOT NULL, fid INT NOT NULL, rating FLOAT NOT NULL, PRIMARY KEY (uid, fid), FOREIGN KEY (uid) REFERENCES Users(id), FOREIGN KEY (fid) REFERENCES Files(id)) "+
                                                "CREATE TABLE Admin (id INT PRIMARY KEY NOT NULL, FOREIGN KEY (id) REFERENCES Users(id)) "+
                                                "INSERT INTO StagedFile VALUES('path') " +
                                                "INSERT INTO Users (userName, password, name, email, balance) VALUES('SmallSon', 'password', 'John Doe', 'john@itu.dk', 0), " +
                                                "('Bigson', 'stortpassword', 'Ikke John Doe', 'lol@itu.dk', 872043), " +
                                                "('DeletionUser', 'password', 'Captain delete', 'dtu@itu.dk', 2323), " +
                                                "('User4', 'password', 'Jack', 'dsatu@itu.dk', 0), " +
                                                "('User5', 'password', 'Joe', 'dtsdasu@itu.dk', 0), " +
                                                "('User6', 'password', 'Joe Admin', 'ku@itu.dk', 0) " +
                                                "INSERT INTO Admin VALUES(1), (6) "+
                                                "INSERT INTO Files VALUES('Life of a Small Son', 10, 25, '', 2013, 'Tells the sad tale of director John Doe''s life as a small son', 'http://4.bp.blogspot.com/-1tmYxvB58CY/UFjMqzTdQKI/AAAAAAAAH_M/g7kWAu4Y4kc/s1600/600px-No_image_available.svg.png',0)," +
                                                "('Life of a Small Son Title Track (Small Son)', 2, 5, 'rentit.itu.dk/RentIt25/downloads/flute.mp3', 2013, 'Awesome flutez', 'http://4.bp.blogspot.com/-1tmYxvB58CY/UFjMqzTdQKI/AAAAAAAAH_M/g7kWAu4Y4kc/s1600/600px-No_image_available.svg.png',0), "+
                                                "('A', 1, 2, '', 2912, 'Hej', 'URILOL', 4000), " +
                                                "('Sang', 55, 345, '', 93, 'dsffd', '.', 1) " +
                                                "INSERT INTO Movie VALUES(1), (3) " +
                                                "INSERT INTO Director VALUES('Ben Hansen'), " +
                                                "('Søren') "+
                                                "INSERT INTO MovieDirector VALUES(1,1),(3,1) " +
                                                "INSERT INTO Song VALUES(2, 'Life of a Small Son Soundtrack'), "+
                                                "(4, 'Xzibits album') " +
                                                "INSERT INTO Artist VALUES ('Vanilla Ice'), ('Xzibit') "+
                                                "INSERT INTO SongArtist VALUES (2,1),(4,2) "+
                                                "INSERT INTO Genre VALUES('Dance') "+
                                                "INSERT INTO GenreFile VALUES(1,2) " + 
                                                "INSERT INTO UserFileRating VALUES(1,1, 10)",
                                                connection);
            command.ExecuteNonQuery();
        }
    }
}
