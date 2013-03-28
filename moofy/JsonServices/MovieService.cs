﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using moofy.Backend;

namespace moofy.JsonServices {
    public partial class MoofyServices : IMovieService {

        public MovieWrapper GetMovie(string id) {
            int mid = Convert.ToInt32(id);
            if (mid > 0) {
                db.Open();
                MovieWrapper movie = db.GetMovie(mid).ToWrapper();
                db.Close();
                return movie;
            } else {
                throw new ArgumentException("Invalid id");
            }
        }

        public SuccessFlag PurchaseMovie(string id, int userId) {
            int mid = Convert.ToInt32(id);
            if (mid > 0 && userId > 0)
                return new SuccessFlag() {
                    success = true,
                    message = "Movie and user ids valid. This has not been implemented yet."
                };
            else
                return new SuccessFlag() {
                    success = false,
                    message = "Must be valid ids."
                };
        }

        public SuccessFlag RentMovie(string id, int userId) {
            int mid = Convert.ToInt32(id);
            if (mid > 0 && userId > 0)
                return new SuccessFlag() {
                    success = true,
                    message = "Movie and user ids valid. This has not been implemented yet."
                };
            else
                return new SuccessFlag() {
                    success = false,
                    message = "Must be valid ids."
                };
        }

        public SuccessFlagDownload DownloadMovie(string id, string userId) {
            int mid = Convert.ToInt32(id);
            int uid = Convert.ToInt32(userId);
            if (mid > 0 && uid > 0) {
                return new SuccessFlagDownload() {
                    success = true,
                    downloadLink = "http://ge.tt/api/1/files/2jRUQya/0/blob?download"
                };
            } else {
                return new SuccessFlagDownload() {
                    success = false,
                    downloadLink = ""
                };
            }
        }

        public MovieWrapper[] ListAllMovies() {
            db.Open();
            MovieWrapper[] result = db.FilterMovies("").ToWrapper();
            db.Close();
            return result;
        }

        public MovieWrapper[] FilterMovies(string filter) {
            if (filter != "emptyList")
                return new MovieWrapper[] {
                    new MovieWrapper() {
                        title = "Skew",
                        release = 2011,
                        genres = new string[] { "Horror", "Thriller" },
                        directors = new string[] { "Sevé Schelenz" },
                        description = "When Simon, Rich, and Eva head out on an eagerly anticipated road trip, they bring along a video camera to record their journey. What starts out as a carefree adventure slowly becomes a descent into the ominous as unexplained events threaten to disrupt the balance between the three close friends. Each one of them must struggle with personal demons and paranoia as friendships are tested and gruesome realities are revealed...and recorded.",
                        rentalPrice = 10,
                        purchasePrice = 30
                    }
                };
            else
                return new MovieWrapper[0];
        }

        public MovieWrapper[] FilterAndSortMovies(string filter, string sortBy) {
            if (filter != "emptyList")
                return new MovieWrapper[] {
                    new MovieWrapper() {
                        title = "Skew",
                        release = 2011,
                        genres = new string[] { "Horror", "Thriller" },
                        directors = new string[] { "Sevé Schelenz" },
                        description = "When Simon, Rich, and Eva head out on an eagerly anticipated road trip, they bring along a video camera to record their journey. What starts out as a carefree adventure slowly becomes a descent into the ominous as unexplained events threaten to disrupt the balance between the three close friends. Each one of them must struggle with personal demons and paranoia as friendships are tested and gruesome realities are revealed...and recorded.",
                        rentalPrice = 10,
                        purchasePrice = 30
                    }
                };
            else
                return new MovieWrapper[0];
        }

        public SuccessFlagUpload UploadMovie(Stream fileStream) {
            return new SuccessFlagUpload() {
                success = true,
                tmpid = "h35fflk9"
            };
        }

        public SuccessFlagId CreateMovie(int managerid, string tmpid, string title, int release, string[] directors, string[] genres, string description, int rentalPrice, int purchasePrice) {
            if (tmpid == "h35fflk9")
                return new SuccessFlagId() {
                    success = true,
                    id = 1
                };
            else throw new ArgumentException("Invalid tmpid - get it by uploading a movie.");
        }

        public SuccessFlag DeleteMovie(string id, int managerid) {
            int mid = Convert.ToInt32(id);
            if (mid > 0 && managerid > 0)
                return new SuccessFlag() {
                    success = true,
                    message = "Both ids valid. This has not yet been implemented"
                };
            else
                return new SuccessFlag() {
                    success = false,
                    message = "Invalid ids"
                };
        }

    }
}
