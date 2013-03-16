using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moofy.Backend {
    class Song {
        //A set of backing fields - must explictly declare accessors Interface properties
        private int id;
        private short year;
        private string title, description, uri;
        private float buyPrice, rentPrice;
        private IList<string> genres;

        public Song(int id) {
            this.id = id;
        }

        public int Id {
            get { return id; }
        }

        public string Title {
            get { return title; }
            set { title = value; }
        }

        public string Description {
            get { return description; }
            set { description = value; }
        }

        public string Artist { get; set; }
        
        public string Album { get; set; }

        public IList<string> Genres {
            get { return genres; }
            set { genres = value; }
        }

        public short Year {
            get { return year; }
            set { year = value; }
        }

        public string Uri {
            get { return uri; }
            set { uri = value; }
        }

        public float BuyPrice {
            get { return buyPrice; }
            set { buyPrice = value; }
        }

        public float RentPrice {
            get { return rentPrice; }
            set { rentPrice = value; }
        }
    }
}
