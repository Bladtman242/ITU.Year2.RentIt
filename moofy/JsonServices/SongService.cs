using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using moofy.Backend;
using System.ServiceModel.Web;

namespace moofy.JsonServices {
    public partial class MoofyServices : ISongService {

        public SongWrapper GetSong(string id) {
            int sid;
            try {
                sid = Convert.ToInt32(id);
            } catch (FormatException e) { // id is not number
                Utils.BadReq("\""+id+"\"" + " is not a number");
                return null;
            }
            if (sid > 0) {
                db.Open();
                Song s = db.GetSong(sid);
                db.Close();
                if (s == null) { // song not found
                    Utils.NotFound("could not find song with id \"" + sid + "\"");
                    return null;
                }
                return s.ToWrapper();
            } else { // id below zero
                Utils.BadReq("id's must be positive numbers");
                return null;
            }
        }

        public SuccessFlag PurchaseSong(string id, int userId) {
            int sid;
            try {
                sid = Convert.ToInt32(id);
            } catch (FormatException e) { //sid is not a number
                Utils.BadReq("\"" + id + "\"" + " is not a number");
                return new SuccessFlag() { success = false, message = id + " doesn't seem like a number" };
            }
                if (sid > 0 && userId > 0) {
                    db.Open();
                    bool res = db.PurchaseSong(sid, userId);
                    db.Close();
                    if (res){ //success
                        return new SuccessFlag() {
                            success = true,
                            message = "Song Purchased"
                        };
                    } else { //song or user not found in db
                        Utils.NotFound("either the user: " + userId + ", or the song: " + sid + "could not be found");
                        return null;
                    }
                } else { // either or both of the id's are less than zero
                    Utils.BadReq("id's must be positive numbers");
                    return new SuccessFlag() {
                        success = false,
                        message = "Id's must be positive (non-zero) integers"
                    };
                }
        }

        public SuccessFlag RentSong(string id, int userId) {
            int sid;
            try{
                sid = Convert.ToInt32(id);
            } catch(FormatException e) { //id is not a number
                Utils.BadReq("\"" + id + "\"" + " is not a number");
                return new SuccessFlag() { success=false, message = id + " doesn't seem to be a number" }; //NaNNaNNaNNaNNaNNaNNaNNaNNaNNaNNaNNaN WATMAN!
            }
            if (sid > 0 && userId > 0) {
                db.Open();
                bool res = db.RentSong(sid, userId, 42); //TODO period
                db.Close();
                if (res) { //success
                    return new SuccessFlag() {
                        success = true,
                        message = "Song rented"
                    };
                } else { //song or user not found
                    Utils.NotFound("either the user: " + userId + ", or the song: " + sid + "could not be found");
                    return null;
                }
            } else { // either or both id's below zero
                Utils.BadReq("id's must be positive numbers");
                return new SuccessFlag() {
                    success = false,
                    message = "Id's must be positive (non-zero) integers"
                };
            }
        }

        public SuccessFlagDownload DownloadSong(string id, string userId) {
            int sid, uid;
            try {
                sid = Convert.ToInt32(id);
                uid = Convert.ToInt32(userId);
            }
            catch (FormatException e) { // not a number
                Utils.BadReq("either \"" + id + "\", or " + "\"" + userId + "\"" + " is not a number");
                return new SuccessFlagDownload() {
                    success = false,
                    downloadLink = ""
                };
            }

            if (sid > 0 && uid > 0) {
                db.Open();
                string downloadLink = db.DownloadFile(sid, uid);
                db.Close();

                if (downloadLink != null && downloadLink !="") {//success
                    return new SuccessFlagDownload() {
                        success = true,
                        downloadLink = downloadLink
                    };
                } else if (downloadLink == "") { //file not found
                    Utils.NotFound("could not find song with id \"" + sid + "\"");
                    return new SuccessFlagDownload { success = false, downloadLink = "" };
                } else { // song not purchased
                    Utils.BadReq("user " + uid + " is not allowed access to song " + sid);
                    return new SuccessFlagDownload { success = false, downloadLink = "" };
                }
            }
            else { // id below zero
                Utils.BadReq("id's must be positive numbers");
                return new SuccessFlagDownload { success = false, downloadLink = "" };
            }
        }

        public SongWrapper[] ListAllSongs() {
            db.Open();  
            SongWrapper[] s = db.FilterSongs("").ToWrapper();
            db.Close();
            return s;
        }

        public SongWrapper[] FilterSongs(string filter) {
                db.Open();
                SongWrapper[] s = db.FilterSongs(filter).ToWrapper();
                db.Close();
                return s;
        }

        public SongWrapper[] FilterAndSortSongs(string filter, string sortBy) {
            db.Open();
            Song[] s = db.FilterSongs(filter);
            db.Close();
            try {
                Sorter.SortBy(s, sortBy);
                return s.ToWrapper();
            } catch (ArgumentException e) {
                Utils.BadReq(e.Message);
                return null;
            }
        }

        public SuccessFlagUpload UploadSong(Stream fileStream) {
            db.Open();
            int id;
            try {
                id = db.UploadFile(fileStream);
            } catch (FormatException e) {
                Utils.BadReq(e.Message);
                return new SuccessFlagUpload() { success = false, tmpid = "" };
            } finally {
                System.Diagnostics.Debug.WriteLine("here");
                db.Close();
           } 
            return new SuccessFlagUpload() {
                success = true,
                tmpid = id.ToString()
            };
        }

        public SuccessFlagId CreateSong(int managerid, int tmpid, string title, string description, int release, string artist, string album, string[] genres, int rentalPrice, int purchasePrice, string coverUri) {
             Song song = new Song()
            {
                Title = title,
                Year = (short)release,
                Description = description,
                Album = album,
                RentPrice = rentalPrice,
                BuyPrice = purchasePrice,
                CoverUri = coverUri
            };
            db.Open();
            IList<string> genr = new List<string>(genres);
            Song song1 = db.CreateSong(managerid, tmpid, genr, song, new List<string>{artist});
            db.Close();
            if (song1 == null) {
                Utils.BadReq("Ensure you entered a valid tmpId and a valid admin id");
                return new SuccessFlagId() { success = false };
            } else {
                return new SuccessFlagId() {
                    id = song1.Id,
                    success = true
                };
            }
        }

        public SuccessFlag DeleteSong(string id, int managerid) {
            int sid;
            try {
                sid = Convert.ToInt32(id);
            } catch (FormatException e) {
                Utils.BadReq(e.Message);
                return new SuccessFlag() { success = false, message = id + " nach uimhir?" };
            }
            if (sid > 0 && managerid > 0) {
                db.Open();
                bool suc = db.DeleteSong(sid, managerid);
                db.Close();
                
                return new SuccessFlag() {
                    success = suc,
                    message = suc ? "" : "Song not found, or user not a manager"
                };
            } else {
                Utils.BadReq("id's must be positive");
                return new SuccessFlag() {
                    success = false,
                    message = "Invalid ids"
                };
            }
        }

        public SuccessFlag RateSong(string id, int userId, int rating) {
            int sid;
            try
            {
                sid = Convert.ToInt32(id);
            }
            catch (FormatException e)
            {
                Utils.BadReq(e.Message);
                return new SuccessFlag() { success = false, message = id + " nach uimhir?" };
            }
            if (sid > 0 && userId > 0)
            {
                db.Open();
                bool suc = db.RateFile(userId, sid, rating);
                db.Close();

                return new SuccessFlag()
                {
                    success = suc, //actual confimation? the 
                    message = suc ? "" : "Rating could not be added"
                };
            }
            else
            {
                Utils.BadReq("id's must be positive");
                return new SuccessFlag()
                {
                    success = false,
                    message = "Invalid ids"
                };
            }

        }

        public RatingWrapper GetSongRatings(string id, string userId) {
            int sid;
            int uid;
            try
            {
                sid = Convert.ToInt32(id);
                uid = Convert.ToInt32(userId);
            }
            catch (FormatException e)
            {
                Utils.BadReq(e.Message);
                return null;
            }
            if (sid > 0 && uid > 0)
            {
                db.Open();
                int rat = db.GetRating(uid, sid);
                db.Close();

                return new RatingWrapper()
                {
                    userId= uid,
                    itemId= sid,
                    rating= rat
                };
            }
            Utils.BadReq("id's must be positive integers");
            return null;
        }

        public SuccessFlag UpdateSong(string id, int managerId, string artist = null, string album = null, string title = null, string description = null, int rentalPrice = -1, int purchasePrice = -1, int release = -1, string coverUri = null, string[] genres = null) {
            if (id == "" || id == null) {
                Utils.BadReq("Must give a valid song id");
                new SuccessFlag() { message = "Must give a valid song id", success = false };
            }

            int sid;
            try {
                sid = Convert.ToInt32(id);
            }
            catch (Exception e) {
                Utils.BadReq("id must be a number "+e.Message);
                return new SuccessFlag() { message = "Must give a valid song id", success = false };
            }

            db.Open();
            Song s = db.GetSong(sid);

            if (s == null) {
                db.Close();
                Utils.NotFound("song " + sid + " not found");
                return new SuccessFlag() {
                    success = false,
                    message = "The song you tried to update does not exit"
                };
            }

            
            if (album != null) s.Album = album;
            if (title != null) s.Title = title;
            if (description != null) s.Description = description;
            if (rentalPrice >= 0) s.RentPrice = rentalPrice;
            if (purchasePrice >= 0) s.BuyPrice = purchasePrice;
            if (release >= 0) s.Year = (short)release;
            if (coverUri != null) s.CoverUri = coverUri;

            bool success = db.UpdateSong(s, managerId);
            if (success) {
                if (genres != null) {
                    db.ClearFileGenres(s.Id);
                    success = db.AddAllGenres(s.Id, genres);
                }
            }
            if (success)
            {
                if (artist != null)
                {
                    db.ClearSongArtists(s.Id);
                    success = db.AddArtist(s.Id, artist);
                }
            }
            db.Close();

            if (success) {
                return new SuccessFlag() {
                    message = "Song data updated succesfully.",
                    success = success
                };
            } else {
                Utils.BadReq("Update of song data failed. You must be manager to update song data.");
                return new SuccessFlag() {
                    message = "Update of song data failed. You must be manager to update song data.",
                    success = false
                };
            }
        }

        
    }
}
