using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moofy.Backend {
    public class User {
        private int id;
        private bool? isAdmin;
        private IList<Purchase> purchases;

        public User(int id) {
            this.id = id;
        }

        public int Id {
            get { return id; }
        }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public float Balance { get; set; }

        public bool IsAdmin {
            get { return (bool)(isAdmin ?? DBUser.getIsAdmin(this.Id)); }
        }

        public IList<Purchase> Purchases {
            get { return purchases ?? DBUser.getPurchases(this.Id); }
        }
    }
}
