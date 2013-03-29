using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace moofy.JsonServices {
    public partial class MoofyServices : ISongService {

        public SongWrapper GetSong(string id) {
            int sid = Convert.ToInt32(id);
            if (sid > 0) {
                return db.GetSong(sid).ToWrapper();
            } else {
                throw new ArgumentException("Invalid id");
            }
        }

        public SuccessFlag PurchaseSong(string id, int userId) {
            int sid = Convert.ToInt32(id);
            if (sid > 0 && userId > 0) {
                db.PurchaseSong(sid, userId);
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
            int sid = Convert.ToInt32(id);
            if (sid > 0 && userId > 0) {
                db.RentSong(sid, userId, 42); //TODO period
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
            int sid = Convert.ToInt32(id);
            int uid = Convert.ToInt32(userId);
            if (sid > 0 && uid > 0) {
                return new SuccessFlagDownload() {
                    success = true,
                    downloadLink = db.GetSong(sid).Uri
                };
            } else {
                return new SuccessFlagDownload() {
                    success = false,
                    downloadLink = ""
                };
            }
        }

        public SongWrapper[] ListAllSongs() {
              return db.FilterSongs("").ToWrapper();
        }

        public SongWrapper[] FilterSongs(string filter) {
            if (filter != "emptyList")
                return new SongWrapper[] {
                    new SongWrapper() {
                        title = "I Knew You Were Trouble",
                        release = 2012,
                        genres = new string[] { "Pop" },
                        album = "Red",
                        artist = "Taylor Swift",
                        rentalPrice = 2,
                        purchasePrice = 8
                    }
                };
            else
                return new SongWrapper[0];
        }

        public SongWrapper[] FilterAndSortSongs(string filter, string sortBy) {
            if (filter != "emptyList")
                return new SongWrapper[] {
                    new SongWrapper() {
                        title = "I Knew You Were Trouble",
                        release = 2012,
                        genres = new string[] { "Pop" },
                        album = "Red",
                        artist = "Taylor Swift",
                        rentalPrice = 2,
                        purchasePrice = 8
                    }
                };
            else
                return new SongWrapper[0];
        }

        public SuccessFlagUpload UploadSong(Stream fileStream) {
            return new SuccessFlagUpload() {
                success = true,
                tmpid = "f1337a12"
            };
        }

        public SuccessFlagId CreateSong(int managerid, string tmpid, string title, int release, string artist, string album, string[] genres, int rentalPrice, int purchasePrice) {
            if (tmpid == "f1337a12")
                return new SuccessFlagId() {
                    success = true,
                    id = 1
                };
            else throw new ArgumentException("Invalid tmpid - get it by uploading a song.");
        }

        public SuccessFlag DeleteSong(string id, int managerid) {
            int sid = Convert.ToInt32(id);
            if (sid > 0 && managerid > 0)
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
