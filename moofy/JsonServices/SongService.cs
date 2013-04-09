using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using moofy.Backend;

namespace moofy.JsonServices {
    public partial class MoofyServices : ISongService {

        public SongWrapper GetSong(string id) {
            int sid;
            try {
                sid = Convert.ToInt32(id);
            } catch (FormatException e) {
                return new SongWrapper(); //dunno how to actually return a 404
            }
            if (sid > 0) {
                db.Open();
                Song s = db.GetSong(sid);
                if (s == null) return null;
                db.Close();
                return s.ToWrapper();
            } else {
                return new SongWrapper();
            }
        }

        public SuccessFlag PurchaseSong(string id, int userId) {
            int sid;
            try {
                sid = Convert.ToInt32(id);
            } catch (FormatException e) {
                return new SuccessFlag() { success = false, message = id + " doesn't seem like a number" };
            }
                if (sid > 0 && userId > 0) {
                    db.Open();
                    db.PurchaseSong(sid, userId);
                    db.Close();
                    return new SuccessFlag() {
                        success = true,
                        message = "Song Purchased"
                    };
                } else {
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
            } catch(FormatException e) {
                return new SuccessFlag() { success=false, message = id + " doesn't seem to be a number" }; //NaNNaNNaNNaNNaNNaNNaNNaNNaNNaNNaNNaN WATMAN!
            }
            if (sid > 0 && userId > 0) {
                db.Open();
                db.RentSong(sid, userId, 42); //TODO period
                db.Close();
                return new SuccessFlag() {
                    success = true,
                    message = "Song rented"
                };
            } else {
                return new SuccessFlag() {
                    success = false,
                    message = "Id's must be positive (non-zero) integers"
                };
            }
        }

        public SuccessFlagDownload DownloadSong(string id, string userId) {
            int sid;
            int uid =1;// = Convert.ToInt32(userId);
            try {
                sid = Convert.ToInt32(id);
            } catch (FormatException e) {
                return new SuccessFlagDownload { success = false, downloadLink = "" };
            }
            if (sid > 0 && uid > 0) {
                db.Open();
                SuccessFlagDownload ret = new SuccessFlagDownload{
                    success = true,
                    downloadLink = db.GetSong(sid).Uri
                };
                db.Close();
                return ret;
            } else {
                return new SuccessFlagDownload() {
                    success = false,
                    downloadLink = ""
                };
            }
        }

        public SongWrapper[] ListAllSongs() {
            db.Open();  
            SongWrapper[] s = db.FilterSongs("").ToWrapper();
            db.Close();
            return s;
        }

        public SongWrapper[] FilterSongs(string filter) {
            if (filter != "emptyList"){
                db.Open();
                SongWrapper[] s = db.FilterSongs(filter).ToWrapper();
                db.Close();
                return s;
            }else{
                return new SongWrapper[0];
            }
        }

        public SongWrapper[] FilterAndSortSongs(string filter, string sortBy) {
            if (filter != "emptyList") {
                db.Open();
                Song[] s = db.FilterSongs(filter);
                try {
                    
                    Sorter.SortBy(s, sortBy);
                    return s.ToWrapper();
                } catch (ArgumentException e){
                    return new SongWrapper[0];
                } finally{
                    db.Close();
                }
            } else {
                return new SongWrapper[0];
            }
        }

        public SuccessFlagUpload UploadSong(Stream fileStream) {
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

        public SuccessFlagId CreateSong(int managerid, int tmpid, string title, string description, int release, string artist, string album, string[] genres, int rentalPrice, int purchasePrice, string coverUri) {
             Song song = new Song()
            {
                Title = title,
                Year = (short)release,
                Description = description,
                Album = album,
                Artist = artist,
                RentPrice = rentalPrice,
                BuyPrice = purchasePrice,
                CoverUri = coverUri
            };
            db.Open();
            IList<string> genr = new List<string>(genres);
            Song song1 = db.CreateSong(managerid, tmpid, genr, song);
            if (song1 == null)
                throw new ArgumentException("Ensure you entered a valid tmpId and a valid admin id");
            else
                return new SuccessFlagId()
                {
                    id = song1.Id,
                    success = true
                };
        }

        public SuccessFlag DeleteSong(string id, int managerid) {
            int sid;
            try {
                sid = Convert.ToInt32(id);
            } catch (FormatException e) {
                return new SuccessFlag() { success = false, message = id + " nach uimhir?" };
            }
            if (sid > 0 && managerid > 0) {
                db.Open();
                bool suc = db.DeleteSong(sid, managerid);
                db.Close();
                
                return new SuccessFlag() {
                    success = suc, //actual confimation? the 
                    message = suc ? "" : "Song not found, or user not a manager"
                };
            } else {
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

            return null;
        }

    }
}
