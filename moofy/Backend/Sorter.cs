using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moofy.Backend {
    public class Sorter {
/*        private static Sorter instance;

        private Sorter() { }

        public static Sorter Instance {
            get {
                if (instance == null) {
                    instance = new Sorter();
                }
                return instance;
            }
        }*/
             
        protected static void ByTitle(File[] files) {
            Array.Sort(files, (f1, f2) => f1.Title.CompareTo(f2.Title));
        }

        protected static void ByDescription(File[] files) {
            Array.Sort(files, (f1, f2) => f1.Description.CompareTo(f2.Description));
        }

        protected static void ByGenre(File[] files) {
            Array.Sort(files, (f1, f2) => f1.Genres[0].CompareTo(f2.Genres[0])); //This is a pretty useless sort.
        }

        protected static void ByYear(File[] files) {
            Array.Sort(files, (f1, f2) => {System.Diagnostics.Debug.WriteLine(f1.Year + ", " + f2.Year + " : " + f1.Year.CompareTo(f2.Year)); return f1.Year.CompareTo(f2.Year);});
        }

        protected static void ByBuyPrice(File[] files) {
            Array.Sort(files, (f1, f2) => f1.BuyPrice.CompareTo(f2.BuyPrice));
        }

        protected static void ByRentPrice(File[] files) {
            Array.Sort(files, (f1, f2) => f1.RentPrice.CompareTo(f2.RentPrice));
        }

        protected static void ByDirector(Movie[] movs) {
            Array.Sort(movs, (m1, m2) => m1.Director.CompareTo(m2.Director)); //This is a pretty useless sort.
        }

        protected static void ByArtist(Song[] songs) {
            Array.Sort(songs, (s1, s2) => s1.Artist.CompareTo(s2.Artist));
        }

        protected static void ByAlbum(Song[] songs) {
            Array.Sort(songs, (s1, s2) => s1.Album.CompareTo(s2.Album));
        }

        public static void SortBy(File[] files, String property) {
            //should probably be done with enums, or by a completely different pattern (visitor?)
            switch (property.ToLower()) {
                case "title":
                    ByTitle(files);
                    break;
                case "description" :
                    ByDescription(files);
                    break;
                case "genre":
                    ByGenre(files);
                    break;
                case "release":
                case "year":
                    ByYear(files);
                    break;
                default:
                    throw new ArgumentException(property + " is undefined");
                    break; //completely unnecessary
            }
        }

        public static void SortBy(Movie[] movs, String property) {
            property = property.ToLower();
            switch (property) {
                case "director":
                case "directors":
                    ByDirector(movs);
                    break;
                default:
                    SortBy((File[])movs, property);
                    break;
            }
        }

        public static void SortBy(Song[] songs, String property) {
            property = property.ToLower();
            switch (property) {
                case "album":
                    ByAlbum(songs);
                    break;
                case "artist":
                    ByArtist(songs);
                    break;
                default :
                    SortBy((File[]) songs, property);
                    break;
            }
        }
    }
}
