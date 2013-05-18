using System;
using System.Net;
using System.ServiceModel.Web;
using moofy.Backend;
using System.Collections.Generic;

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
                    message = res.Id>0 ? "" +res.Id : "An error occured on the server. Could not register user, please try again."
                };
            }else{
                return new SuccessFlag() {
                    success = false,
                    message = "One or more fields are empty, please try again with data in all fields"
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
            int uid = Convert.ToInt32(id);
            if (uid <= 0) throw new ArgumentException("User id must be greater than 0.");

            List<MovieWrapper> movieList = new List<MovieWrapper>();
            db.Open();
            User u = db.GetUser(uid);
            IList<Purchase> purchases = u.Movies;
            foreach (Purchase p in purchases) {
                movieList.Add(db.GetMovie(p.File.Id).ToWrapper());
            }
            db.Close();

            return movieList.ToArray();
        }

        public MovieWrapper[] GetCurrentMoviesFromUser(string id) {
            int uid = Convert.ToInt32(id);
            if (uid <= 0) throw new ArgumentException("User id must be greater than 0.");

            List<MovieWrapper> movieList = new List<MovieWrapper>();
            db.Open();
            User u = db.GetUser(uid);
            IList<Purchase> purchases = u.Movies;
            foreach (Purchase p in purchases) {
                if (p.EndTime > DateTime.Now)
                    movieList.Add(db.GetMovie(p.File.Id).ToWrapper());
            }
            db.Close();

            return movieList.ToArray();
        }

        public SongWrapper[] GetSongsFromUser(string id) {
            int uid = Convert.ToInt32(id);
            if (uid <= 0) throw new ArgumentException("User id must be greater than 0.");

            List<SongWrapper> songList = new List<SongWrapper>();
            db.Open();
            User u = db.GetUser(uid);
            IList<Purchase> purchases = u.Songs;
            foreach (Purchase p in purchases) {
                songList.Add(db.GetSong(p.File.Id).ToWrapper());
            }
            db.Close();

            return songList.ToArray();
        }

        public SongWrapper[] GetCurrentSongsFromUser(string id) {
            int uid = Convert.ToInt32(id);
            if (uid <= 0) throw new ArgumentException("User id must be greater than 0.");

            List<SongWrapper> songList = new List<SongWrapper>();
            db.Open();
            User u = db.GetUser(uid);
            IList<Purchase> purchases = u.Songs;
            foreach (Purchase p in purchases) {
                if (p.EndTime < DateTime.Now)
                    songList.Add(db.GetSong(p.File.Id).ToWrapper());
            }
            db.Close();

            return songList.ToArray();
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

            bool success = db.UpdateUser(u);
            db.Close();

            return new SuccessFlag() {
                success = success,
                message = success ? "Succesfully updated user." : "Failed to update user info!"
            };
        }

    }
}
