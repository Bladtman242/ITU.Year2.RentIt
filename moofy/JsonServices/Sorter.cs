﻿using System;
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
            Array.Sort(files, (f1, f2) => f1.Year.CompareTo(f2.Year));
        }

        protected static void ByBuyPrice(File[] files) {
            Array.Sort(files, (f1, f2) => f1.BuyPrice.CompareTo(f2.BuyPrice));
        }

        protected static void ByRentPrice(File[] files) {
            Array.Sort(files, (f1, f2) => f1.RentPrice.CompareTo(f2.RentPrice));
        }

        protected static void ByRating(File[] files) {
            Array.Sort(files, (f1, f2) => -1 * f1.AverageRating.CompareTo(f2.AverageRating)); // *-1 to order descending
        }

        protected static void ByViews(File[] files) {
            Array.Sort(files, (f1, f2) => -1 * f1.ViewCount.CompareTo(f2.ViewCount));  // *-1 to order descending
            
        }

        protected static void ByDirector(Movie[] movs) {
            Array.Sort(movs, (m1, m2) => m1.Directors.First().CompareTo(m2.Directors.First())); //This is a pretty useless sort.
        }

        protected static void ByArtist(Song[] songs) {
            Array.Sort(songs, (s1, s2) => s1.Artists.First().CompareTo(s2.Artists.First()));
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
                case "averagerating":
                case "averageratings":
                case "avgrating":
                case "avgratings":
                    ByRating(files);
                    break;
                case "views":
                case "view":
                case "viewcount":
                case "viewcounts":
                    ByViews(files);
                    break;
                default:
                    throw new ArgumentException(property + " is undefined");
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
