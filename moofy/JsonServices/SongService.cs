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
                return new SongWrapper() {
                    title = "I Knew You Were Trouble",
                    release = 2012,
                    genres = new string[] { "Pop" },
                    album = "Red",
                    artist = "Taylor Swift",
                    rentalPrice = 2,
                    purchasePrice = 8
                };
            } else {
                throw new ArgumentException("Invalid id");
            }
        }

        public SuccessFlag PurchaseSong(string id, int userId) {
            int sid = Convert.ToInt32(id);
            if (sid > 0 && userId > 0)
                return new SuccessFlag() {
                    success = true,
                    message = "Song and user ids valid. This has not been implemented yet."
                };
            else
                return new SuccessFlag() {
                    success = false,
                    message = "Must be valid ids."
                };
        }

        public SuccessFlag RentSong(string id, int userId) {
            int sid = Convert.ToInt32(id);
            if (sid > 0 && userId > 0)
                return new SuccessFlag() {
                    success = true,
                    message = "Song and user ids valid. This has not been implemented yet."
                };
            else
                return new SuccessFlag() {
                    success = false,
                    message = "Must be valid ids."
                };
        }

        public SuccessFlagDownload DownloadSong(string id, string userId) {
            int sid = Convert.ToInt32(id);
            int uid = Convert.ToInt32(userId);
            if (sid > 0 && uid > 0) {
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

        public SongWrapper[] ListAllSongs() {
            return new SongWrapper[] {
                new SongWrapper() {
                    title = "I Knew You Were Trouble",
                    release = 2012,
                    genres = new string[] { "Pop" },
                    album = "Red",
                    artist = "Taylor Swift",
                    rentalPrice = 2,
                    purchasePrice = 8
                },
                new SongWrapper() {
                    title = "Fiskeengel",
                    release = 2003,
                    genres = new string[] { "Pop", "Rock" },
                    album = "Vivaldi And Ven",
                    artist = "Roben Og Knud",
                    rentalPrice = 7,
                    purchasePrice = 15
                },
                new SongWrapper() {
                    title = "Shake That",
                    release = 2005,
                    genres = new string[] { "Hip Hop" },
                    album = "Curtain Call",
                    artist = "Eminem",
                    rentalPrice = 12,
                    purchasePrice = 30
                },
                new SongWrapper() {
                    title = "7 Days",
                    release = 2001,
                    genres = new string[] { "Pop" },
                    album = "Born To Do It",
                    artist = "Craig David",
                    rentalPrice = 5,
                    purchasePrice = 18
                },
                new SongWrapper() {
                    title = "Onani",
                    release = 2004,
                    genres = new string[] { "Disco" },
                    album = "Ganz Geil",
                    artist = "Dario Von Slutty",
                    rentalPrice = 7,
                    purchasePrice = 21
                },
                new SongWrapper() {
                    title = "Pon de Replay",
                    release = 2005,
                    genres = new string[] { "Pop" },
                    album = "Music Of The Sun",
                    artist = "Rihanna",
                    rentalPrice = 10,
                    purchasePrice = 33
                }
            };
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
