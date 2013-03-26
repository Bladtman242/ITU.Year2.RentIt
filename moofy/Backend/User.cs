using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moofy.Backend {
    public class User {
        private int id;
        private bool? isAdmin;
        private IList<Purchase> purchases;
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
        /// The purchases of the User
        /// </summary>
        public IList<Purchase> Purchases {
            get {
                if ((object)purchases == null) {
                    db.Open();
                    purchases = db.GetPurchases(this.Id);
                    db.Close();
                }
                return purchases;
            }
        }
    }
}
