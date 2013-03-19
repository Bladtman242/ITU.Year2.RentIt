using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moofy.Backend {
    public abstract class File {
        //A set of backing fields - must explictly declare accessors Interface properties
        protected int id;
        protected short year;
        protected string title, description, uri;
        protected float buyPrice, rentPrice;
        protected IList<string> genres;

        public File(int id) {
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

        //Equals override according to http://msdn.microsoft.com/en-us/library/ms173147(v=vs.80).aspx
        //This will mainly be used in testing
        public override bool Equals(System.Object obj) {
            // If parameter is null return false.
            if (obj == null) {
                return false;
            }

            // If parameter cannot be cast to File return false.
            File f = obj as File;
            if ((System.Object)f == null) {
                return false;
            }

            // Return true if the fields match:
            return (Id == f.Id)
                    && (Title == f.Title)
                    && (Description == f.Description)
                    && (Genres == f.Genres)
                    && (Year == f.Year)
                    && (Uri == f.Uri)
                    && (BuyPrice == f.BuyPrice)
                    && (RentPrice == f.RentPrice);
        }

        public bool Equals(File f) {
            // If parameter is null return false:
            if ((object)f == null) {
                return false;
            }

            // Return true if the fields match:
            return (Id == f.Id)
                    && (Title == f.Title)
                    && (Description == f.Description)
                    && (Genres == f.Genres)
                    && (Year == f.Year)
                    && (Uri == f.Uri)
                    && (BuyPrice == f.BuyPrice)
                    && (RentPrice == f.RentPrice);
        }

        public override int GetHashCode() {
            return Id;
        }
    }
}
