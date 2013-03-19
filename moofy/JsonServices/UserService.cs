using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moofy.JsonServices {
    public partial class MoofyServices : IUserService {

        public SuccessFlag CreateUser(string name, string username, string email, string password) {
            if (name != "" && username != "" && email != "" && password != "")
                return new SuccessFlag() {
                    success = true,
                    message = "Fields have correct data. This has not yet been implemented."
                };
            else
                return new SuccessFlag() {
                    success = false,
                    message = "Not all fields have values non-empty."
                };
        }

        public UserWrapper GetUser(string id) {
            int uid = Convert.ToInt32(id);
            if (uid > 0)
                return new UserWrapper() {
                    name = "John Doe",
                    username = "johnd0",
                    email = "john.doe@doecorp.com",
                    balance = 1337
                };
            else
                throw new ArgumentException("The specified user does not exist");
        }

        public SuccessFlagId Login(string username, string password) {
            if (username != "" && password != "")
                return new SuccessFlagId() {
                    success = true,
                    id = 1
                };
            else
                return new SuccessFlagId() {
                    success = false,
                    id = 0
                };
        }

        public SuccessFlag DepositMoney(string id, int moneyAmount) {
            int uid = Convert.ToInt32(id);
            if (uid > 0 && moneyAmount > 0)
                return new SuccessFlag() {
                    success = true,
                    message = "Id and money amount valid. This has not been implemented yet."
                };
            else
                return new SuccessFlag() {
                    success = false,
                    message = "Must be valid id and deposit positive amount of money."
                };
        }

        public MovieWrapper[] GetMoviesFromUser(string id) {
            int uid = Convert.ToInt32(id);
            if (uid > 0)
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
            else throw new ArgumentException("Illegal id");
        }

        public MovieWrapper[] GetCurrentMoviesFromUser(string id) {
            int uid = Convert.ToInt32(id);
            if (uid > 0)
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
            else throw new ArgumentException("Illegal id");
        }

        public SongWrapper[] GetSongsFromUser(string id) {
            int uid = Convert.ToInt32(id);
            if (uid > 0)
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
            else throw new ArgumentException("Illegal id");
        }

        public SongWrapper[] GetCurrentSongsFromUser(string id) {
            int uid = Convert.ToInt32(id);
            if (uid > 0)
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
            else throw new ArgumentException("Illegal id");
        }

    }
}
