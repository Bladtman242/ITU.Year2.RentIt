using System;
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
            if (mid > 0 && userId > 0) {
                db.PurchaseMovie(mid, userId);
                return new SuccessFlag() {
                    success = true,
                    message = "Movie puchased"
                };
            } else {
                return new SuccessFlag() {
                    success = false,
                    message = "Id's must be positive (non-zero) integers"
                };
            }
        }

        public SuccessFlag RentMovie(string id, int userId) {
            int mid;
            try {
                mid = Convert.ToInt32(id);
            } catch (FormatException e) {
                return new SuccessFlag() {success = false, message=id +" doesn't seem like a number"};
            }

            if (mid > 0 && userId > 0) {
                db.RentMovie(mid, userId, 42); //yes, forty-two. Why are you looking at me like that?
                return new SuccessFlag() {
                    success = true,
                    message = "Movie rented"
                };
            } else {
                return new SuccessFlag() {
                    success = false,
                    message = "Id's must be positive (non-zero) integers"
                };
            }
        }

        public SuccessFlagDownload DownloadMovie(string id, string userId) {
            int mid = Convert.ToInt32(id);
            int uid = Convert.ToInt32(userId);
            if (mid > 0 && uid > 0) {
                return new SuccessFlagDownload() {
                    success = true,
                    downloadLink = db.GetMovie(mid).Uri //shouldn't I check if the user has paid for this?
                };
            } else {
                return new SuccessFlagDownload() {
                    success = false,
                    downloadLink = "Id's must be positive (non-zero) integers"
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
            if (filter != "emptyList") {//Why the hell would someone use that for a filter?
                return db.FilterMovies(filter).ToWrapper();

            } else {
                return new MovieWrapper[0];
            }
        }


        public MovieWrapper[] FilterAndSortMovies(string filter, string sortBy) {
            if (filter != "emptyList") {
                Movie[] movs= db.FilterMovies(filter);
                try {
                    Movie.SortBy(movs, sortBy);
                } catch (ArgumentException e) {
                    //sortby property invalid
                    return new MovieWrapper[0];
                }
                return movs.ToWrapper();
            } else {
                return new MovieWrapper[0];
            }
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
            if (mid > 0 && managerid > 0){
                db.DeleteMovie(mid, managerid);
                return new SuccessFlag() {
                    success = true,
                    message = "Movie deleted"
                };
        }else{
                return new SuccessFlag() {
                    success = false,
                    message = "Id's must be positive (non-zero) integers"
                };
    }
        }

    }
}
