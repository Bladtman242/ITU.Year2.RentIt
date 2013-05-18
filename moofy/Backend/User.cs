using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moofy.Backend {
    public class User {
        private bool? isAdmin;
        private IList<Purchase> songs, movies;
        private DBAccess db;

        public User() {
            db = new DBAccess();
        }

        /// <summary>
        /// The id of the User
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The username of the User
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password of the User
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The name of the User
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The email of the User
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The balance on the user's account
        /// </summary>
        public int Balance { get; set; }

        /// <summary>
        /// Whether or not this user is an admin
        /// </summary>
        public bool IsAdmin {
            get {
                if ((object)isAdmin == null) {
                    db.Open();
                    isAdmin = db.GetIsAdmin(this.Id);
                    db.Close();
                }
                return (bool)isAdmin;
            }
        }

        /// <summary>
        /// The songs of the User
        /// </summary>
        public IList<Purchase> Songs {
            get {
                if ((object)songs == null) {
                    db.Open();
                    songs = db.GetSongs(this.Id);
                    db.Close();
                }
                return songs;
            }
        }

        /// <summary>
        /// The movies of the User
        /// </summary>
        public IList<Purchase> Movies {
            get {
                if ((object)movies == null) {
                    db.Open();
                    movies = db.GetMovies(this.Id);
                    db.Close();
                }
                return movies;
            }
        }
    }
}
