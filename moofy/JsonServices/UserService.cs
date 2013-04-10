using System;
using System.Net;
using System.ServiceModel.Web;
using moofy.Backend;

namespace moofy.JsonServices {
    public partial class MoofyServices : IUserService {

        public SuccessFlag CreateUser(string name, string username, string email, string password) {
            if (name != "" && username != "" && email != "" && password != "") {
                db.Open();
                User res = db.AddUser(new User(){ Name=name, Username=username, Email=email, Password=password});
                db.Close();
                if (res == null)
                {
                    return new SuccessFlag()
                    {
                        success = false,
                        message = "The chosen username is already registered in the system. Please choose another."
                    };
                }
                return new SuccessFlag() {
                    success = res.Id>0,
                    message = res.Id>0 ? "assigned id: " +res.Id : "An error occured on the server. Could not register user"
                };
            }else{
                return new SuccessFlag() {
                    success = false,
                    message = "One or more fields are empty"
                };
            }
        }

        public UserWrapper GetUser(string id) {
            int uid;
            try {
                uid = Convert.ToInt32(id);
            } catch (FormatException e) {
                WebOperationContext.Current.OutgoingResponse.SetStatusAsNotFound();
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NotFound; //this does something intelligent, but it doesn't look like a 404 in the tests.
                WebOperationContext.Current.OutgoingResponse.StatusDescription = id + " does not seem to be a number";
                return null;
            }
            if (uid > 0){
                db.Open();
                UserWrapper s = db.GetUser(uid).ToWrapper();
                db.Close();
                return s;
            }else{
                return null; //better than crashing the service
            }
        }

        public SuccessFlagId Login(string username, string password) {
            if (username != "" && password != "") {
                db.Open();
                int suc = db.Login(username, password);
                db.Close();
                return new SuccessFlagId() {
                    success = suc > 0,
                    id = suc > 0 ? suc : -1
                };
            } else {
                return new SuccessFlagId() {
                    success = false,
                    id = 0
                };
            }
        }

        public SuccessFlag DepositMoney(string id, int moneyAmount) {
            int uid;
            try {
                uid = Convert.ToInt32(id);
            } catch (FormatException e) {
                return new SuccessFlag() { success = false, message = id + " ? If that is a number, I'm john wayne" };
            }
            if (uid > 0 && moneyAmount > 0) { //not sure deposit of 0 makes more sense than negative ones
                db.Open();
                db.Deposit(moneyAmount, uid);
                db.Close();
                return new SuccessFlag() {
                    success = true,
                    message = "Yay, deposit successful"
                };
            } else {
                return new SuccessFlag() {
                    success = false,
                    message = "Must be valid id and deposit positive amount of money."
                };
            }
        }

        public MovieWrapper[] GetMoviesFromUser(string id) {
            int uid;
            try {
                uid = Convert.ToInt32(id);
            } catch (FormatException e) {
                return null;
            }

            if (uid > 0) {
                db.Open();
                db.GetMovies(uid);
                db.Close();
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
            } else {
                return null;
            }
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

        public SuccessFlag UpdateUser(string id, string name, string email, string password) {
            if (id == null || id == "") throw new ArgumentException("Must supply an id.");
            
            int uid;
            try {
                uid = Convert.ToInt32(id);
            }
            catch (Exception e) {
                throw new ArgumentException("User id must be an integer!", e);
            }

            db.Open();
            User u = db.GetUser(uid);

            if (name != null) u.Name = name;
            if (email != null) u.Email = email;
            if (password != null) u.Password = password;

            db.UpdateUser(u);
            db.Close();

            return new SuccessFlag() {
                success = true,
                message = "Succesfully updated user."
            };
        }

    }
}
