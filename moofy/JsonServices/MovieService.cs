﻿using System;
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
            if (userId == "" || id == "" || userId == null || id == null) throw new ArgumentException("Invalid value for id or userId, must supply both.");

            int mid, uid;
            try {
                mid = Convert.ToInt32(id);
                uid = Convert.ToInt32(userId);
            } catch (FormatException e) {
                throw new ArgumentException("Invalid format of movie id or user id - must be integer!", e);
            }

            if (mid > 0 && uid > 0) {
                db.Open();
                string downloadLink = db.DownloadFile(mid, uid);
                db.Close();

                if (downloadLink != null) {
                    return new SuccessFlagDownload() {
                        success = true,
                        downloadLink = downloadLink
                    };
                }
                else {
                    throw new AccessViolationException("You do not have the permission to download this movie.");
                }
            } else {
                throw new ArgumentException("Movie and user ids must both be greater than 0.");
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

        public MovieWrapper[] SortMovies(string sortBy) {
            return FilterAndSortMovies("", sortBy);
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

        public SuccessFlagId CreateMovie(int managerid, int tmpid, string title, int release, string[] directors, string[] genres, string description, int rentalPrice, int purchasePrice, string coverUri) {

            Movie mov = new Movie()
            {
                Title = title,
                Year = (short)release,
                Description = description,
                RentPrice = rentalPrice,
                BuyPrice = purchasePrice,
                CoverUri = coverUri
            };
            db.Open();
            IList<string> genr = new List<string>(genres);
            Movie mov1 = db.CreateMovie(managerid, tmpid, genr, mov, directors);
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

        public SuccessFlag UpdateMovie(string id, int managerId, string title = null, string description = null, int rentalPrice = -1, int purchasePrice = -1, int release = -1, string coverUri = null, string[] genres = null, string[] directors = null) {
            if (id == "" || id == null) throw new ArgumentNullException("Must give a valid movie id");

            int mid;
            try {
                mid = Convert.ToInt32(id);
            }
            catch (Exception e) {
                throw new ArgumentException("id must be a number", e);
            }

            db.Open();
            Movie m = db.GetMovie(mid);

            if (m == null) {
                db.Close();
                return new SuccessFlag() {
                    success = false,
                    message = "The movie you tried to update does not exit"
                };
            }

            if (title != null) m.Title = title;
            if (description != null) m.Description = description;
            if (rentalPrice >= 0) m.RentPrice = rentalPrice;
            if (purchasePrice >= 0) m.BuyPrice = purchasePrice;
            if (release >= 0) m.Year = (short)release;
            if (coverUri != null) m.CoverUri = coverUri;
            

            bool success = db.UpdateMovie(m, managerId);
            if (success)
            {
                if (genres != null)
                {
                    db.ClearFileGenres(m.Id);
                    success = db.AddAllGenres(m.Id, genres);
                }
            }
            if(success) {
                if(directors != null)
                {
                    db.ClearMovieDirectors(m.Id);
                    success =db.AddAllDirectors(m.Id, directors);
                }
            }
            db.Close();

            return new SuccessFlag() {
                message = success ? "Movie data updated succesfully." : "Update of movie data failed. You must be manager to update movie data.",
                success = success
            };

        }

    }
}
