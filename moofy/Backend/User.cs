using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moofy.Backend {
   public class User {
        private int id;

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

        public bool isAdmin { get; set; }

        public IList<Purchase> Purchases { get; set; }
    }
}
