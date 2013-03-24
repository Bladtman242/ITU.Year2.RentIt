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

        public User(int id) {
            this.id = id;
            db = new DBAccess();
        }

        /// <summary>
        /// The id of the User
        /// </summary>
        public int Id {
            get { return id; }
        }

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
                    isAdmin = db.getIsAdmin(this.Id);
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
                    purchases = db.getPurchases(this.Id);
                    db.Close();
                }
                return purchases;
            }
        }

        //Equals override according to http://msdn.microsoft.com/en-us/library/ms173147(v=vs.80).aspx
        public override bool Equals(System.Object obj) {
            // If parameter is null return false.
            if (obj == null) {
                return false;
            }

            // If parameter cannot be cast to User return false.
            User u = obj as User;
            if ((System.Object)u == null) {
                return false;
            }

            // Return true if the fields match:
            return (Id == u.Id)
                    && (Username.Equals(u.Username))
                    && (Password.Equals(u.Password))
                    && (Name.Equals(u.Name))
                    && (Email.Equals(u.Email))
                    && (Balance == u.Balance)
                    && (IsAdmin == u.IsAdmin)
                    && (Purchases.Equals(u.Purchases));
        }

        /// <summary>
        /// A supplement to the normal equals method, which takes a User parameter instead.
        /// It is recommended by Microsoft, that in addition to implementing Equals(object), Equals(type) should also be implemented for the class's own type in order to enhance performance.
        /// </summary>
        /// <param name="u">The File to compare</param>
        /// <returns>True if the two objects are equal</returns>
        public bool Equals(User u) {
            // If parameter is null return false:
            if ((object)u == null) {
                return false;
            }

            // Return true if the fields match:
            return (Id == u.Id)
                     && (Username.Equals(u.Username))
                     && (Password.Equals(u.Password))
                     && (Name.Equals(u.Name))
                     && (Email.Equals(u.Email))
                     && (Balance == u.Balance)
                     && (IsAdmin == u.IsAdmin)
                     && (Purchases.Equals(u.Purchases));
        }

        public override int GetHashCode() {
            return Id;
        }


    }
}
