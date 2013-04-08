using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using moofy.Backend;

namespace moofy.JsonServices {
    public partial class MoofyServices : IMovieService {

        public MovieWrapper GetMovie(string id) {
            int mid;
            try {
                mid = Convert.ToInt32(id);
            } catch (FormatException) {
                return null;
            }
            if (mid > 0) {
                db.Open();
                Movie movie = db.GetMovie(mid);
                if (movie == null) return null;
                db.Close();
                return movie.ToWrapper();
            } else {
                return null;
            }
        }

        public SuccessFlag PurchaseMovie(string id, int userId) {
            int mid;
            try {
                mid = Convert.ToInt32(id);
            } catch (FormatException e) {
                return null;
            }
            if (mid > 0 && userId > 0) {
                db.Open();
                bool suc = db.PurchaseMovie(mid, userId);
                db.Close();
                return new SuccessFlag() {
                    success = suc,
                    message = suc ? "Movie puchased" : "Could not buy te movie"
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
                db.Open();
                bool suc = db.RentMovie(mid, userId, 42); //yes, forty-two. Why are you looking at me like that?
                db.Close();
                return new SuccessFlag() {
                    success = suc,
                    message = suc ? "Movie rented" : "Couldn't rent movie"
                };
            } else {
                return new SuccessFlag() {
                    success = false,
                    message = "Id's must be positive (non-zero) integers"
                };
            }
        }

        public SuccessFlagDownload DownloadMovie(string id, string userId) {
            int mid/*, uid*/;
            try {
                mid = Convert.ToInt32(id);
                //uid = Convert.ToInt32(userId);
            } catch (FormatException e) {
                return new SuccessFlagDownload { downloadLink = "", success = false };
            }

            if (mid > 0 /*&& uid > 0*/) {
                db.Open();
                SuccessFlagDownload ret = new SuccessFlagDownload() {
                    success = true,
                    downloadLink = db.GetMovie(mid).Uri //has the user paid for this?
                };
                db.Close();
                return ret;
            } else {
                return new SuccessFlagDownload() { success = false, downloadLink = "" };
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
                db.Open();
                MovieWrapper[] mw = db.FilterMovies(filter).ToWrapper();
                db.Close();
                return mw;
            } else {
                return new MovieWrapper[0];
            }
        }


        public MovieWrapper[] FilterAndSortMovies(string filter, string sortBy) {
            if (filter != "emptyList") {
                db.Open();
                Movie[] movs= db.FilterMovies(filter);
                try {
                    Sorter.SortBy(movs, sortBy);
                } catch (ArgumentException e) {
                    //sortby property invalid
                    return new MovieWrapper[0];
                }
                finally { db.Close(); }

                return movs.ToWrapper();
            } else {
                return new MovieWrapper[0];
            }
        }

        public SuccessFlagUpload UploadMovie(Stream fileStream) {

            try
            {
                db.Open();
                int id = db.UploadFile(fileStream);
                db.Close();
                return new SuccessFlagUpload()
                {
                    success = true,
                    tmpid = id.ToString()
                };
            }
            catch (Exception)
            {
                return new SuccessFlagUpload()
                {
                    success = false,
                    tmpid = null
                };
            }
        }

        public SuccessFlagId CreateMovie(int managerid, int tmpid, string title, int release, string[] directors, string[] genres, string description, int rentalPrice, int purchasePrice) {

            Movie mov = new Movie()
            {
                Title = title,
                Year = (short)release,
                Director = String.Join(" , ", directors),
                Description = description,
                RentPrice = rentalPrice,
                BuyPrice = purchasePrice
            };
            db.Open();
            IList<string> genr = new List<string>(genres);
            Movie mov1 = db.CreateMovie(managerid, tmpid, genr, mov);
            if (mov1 == null)
                throw new ArgumentException("Ensure you entered a valid tmpId and a valid admin id");
            else
                return new SuccessFlagId()
                {
                    id = mov1.Id,
                    success = true
                };
        }

        public SuccessFlag DeleteMovie(string id, int managerid) {
            int mid;
            try {
                mid = Convert.ToInt32(id);
            } catch (FormatException e) {
                return new SuccessFlag() { success = false, message = id + " is not a number" };
            }
            if (mid > 0 && managerid > 0){
                db.Open();
                bool suc = db.DeleteMovie(mid, managerid);
                db.Close();
                return new SuccessFlag() {
                    success = suc,
                    message = suc ? "Movie deleted" : "Couldn't delete movie"
                };
        }else{
                return new SuccessFlag() {
                    success = false,
                    message = "Id's must be positive (non-zero) integers"
                };
    }
        }

        public SuccessFlag RateMovie(string id, int userId, int rating) {
            int mid;
            try
            {
                mid = Convert.ToInt32(id);
            }
            catch (FormatException e)
            {
                return new SuccessFlag() { success = false, message = id + " nach uimhir?" };
            }
            if (mid > 0 && userId > 0)
            {
                db.Open();
                bool suc = db.RateFile(userId, mid, rating);
                db.Close();

                return new SuccessFlag()
                {
                    success = suc, //actual confimation? the 
                    message = suc ? "" : "Rating could not be added."
                };
            }
            else
            {
                return new SuccessFlag()
                {
                    success = false,
                    message = "Invalid ids"
                };
            }
        }

        public RatingWrapper GetMovieRatings(string id, string userId) {
            int mid;
            int uid;
            try
            {
                mid = Convert.ToInt32(id);
                uid = Convert.ToInt32(userId);
            }
            catch (FormatException e)
            {
                return null;
            }
            if (mid > 0 && uid > 0)
            {
                db.Open();
                int rat = db.GetRating(uid, mid);
                db.Close();

                return new RatingWrapper()
                {
                    userId= uid,
                    itemId= mid,
                    rating= rat
                };
            }

            return null;
        }

    }
}
