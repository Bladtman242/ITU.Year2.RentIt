using moofy.JsonServices;
using System;
using System.Net;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Description;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace moofy.JsonServices.Tests {

    [TestClass]
    public class UnitTest1 {
        private String users = "http://localhost:8732/moofytest/users/";
        private String movies = "http://localhost:8732/moofytest/movies/";
        private String songs = "http://localhost:8732/moofytest/songs/";

        //static ServiceHost host = null;

        //This should probably be a test-utility class on its own
        //[AssemblyInitialize]
        //public static void InitiateServices(TestContext t) {
        //    Uri baseAddr = new Uri("http://localhost:8732/moofytest/");
        //    string addr = "users";
        //    host = new ServiceHost(typeof(MoofyServices), baseAddr);
        //    host.AddServiceEndpoint(typeof(IUserService), new WebHttpBinding("system.web"), addr);
        //    host.Open();
        //}

        //[AssemblyCleanup]
        //public static void KillServices() {
        //    Thread.Sleep(300);
        //    host.Close();
        //}

        //[TestMethod]
        //public void TestMethod1() {
        //    Uri u = new Uri("http://localhost:8732/moofytest/users/1");
        //    WebRequest r = WebRequest.Create(u);
        //    WebResponse res = r.GetResponse();
        //    System.Console.WriteLine(res);
        //}

        /*
         * moofytest/users/
         */
        
        [TestMethod]
        public void UsersValidInUseIdOne(){
            string u = users + "1";
            var res = GetMessage(u);

            string conts = res.contents;
            HttpStatusCode status = res.status;
            
            bool isJsonUser = IsUser(conts);
            Assert.IsTrue(isJsonUser, conts + " is not a valid user");
            Assert.AreEqual(HttpStatusCode.OK, res.status); //assert 200
        }

        [TestMethod]
        public void UsersValidInUseIdLarge() {
            string u = users + "70000";
            var res = GetMessage(u);
            string conts = res.contents;

            bool isJsonUser = IsUser(conts);
            Assert.IsTrue(isJsonUser, conts + " is not a valid user");
            Assert.AreEqual(HttpStatusCode.OK, res.status); //assert 200
        }

        [TestMethod]
        public void UsersValidVacantId2(){
            string u = users +"2";
            var res = GetMessage(u);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void UsersInValidIdZero() {
            string u = users + "0";
            var res = GetMessage(u);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void UsersInValidIdNegOne() {
            string u = users + "-1";
            var res = GetMessage(u);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void UsersInValidIdNeg7k() {
            string u = users + "-70000";
            var res = GetMessage(u);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void UsersInValidTypeTwenty() {
            string u = users + "twenty";
            var res = GetMessage(u);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void UsersInValidTypeKatmandu() {
            string u = users + "Katmandu";
            var res = GetMessage(u);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        /*
         * moofytest/users/movies/
         */

        [TestMethod]
        public void UsersMoviesValidInUseIdOne() {
            string umovs = users + "1/movies";
            var res = GetMessage(umovs);

            string conts = res.contents;
            HttpStatusCode status = res.status;

            bool isJsonMovList = IsMovList(conts);
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsTrue(isJsonMovList, conts + " is not a valid movie list");
        }

        [TestMethod]
        public void UsersMoviesValidInUseId7k() {
            string umovs = users + "70000/movies";
            var res = GetMessage(umovs);

            string conts = res.contents;
            HttpStatusCode status = res.status;

            bool isJsonMovList = IsMovList(conts);
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsTrue(isJsonMovList, conts + " is not a valid movie list");
        }

        [TestMethod]
        public void UsersMoviesValidInUseNoMovs() {
            string umovs = users + "42/movies";
            var res = GetMessage(umovs);

            string conts = res.contents;
            HttpStatusCode status = res.status;

            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.AreEqual(conts,"[]"); //this it?
        }

        [TestMethod]
        public void UsersMoviesValidVacantIdTwo() {
            string umovs = users + "2/movies";
            var res = GetMessage(umovs);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void UsersMoviesInValidIdNegOne() {
            string umovs = users + "-1/movies";
            var res = GetMessage(umovs);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void UsersMoviesInValidIdNeg7k() {
            string umovs = users + "-70000/movies";
            var res = GetMessage(umovs);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void UsersMoviesInValidTypeTwenty() {
            string umovs = users + "twenty/movies";
            var res = GetMessage(umovs);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void UsersMoviesInValidTypeKatmandu() {
            string umovs = users + "katmandu/movies";
            var res = GetMessage(umovs);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        /*
         * moofytest/users/movies/current
         */

        [TestMethod]
        public void UsersMoviesCurrentValidInUseIdOneWithMovs() {
            string umovs = users + "1/movies/current";
            var res = GetMessage(umovs);

            string conts = res.contents;
            HttpStatusCode status = res.status;

            bool isJsonMovList = IsMovList(conts);
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsTrue(isJsonMovList, conts + " is not a valid movie list");
        }

        [TestMethod]
        public void UsersMoviesCurrentValidInUseId7kWithMovs() {
            string umovs = users + "70000/movies/current";
            var res = GetMessage(umovs);

            string conts = res.contents;
            HttpStatusCode status = res.status;

            bool isJsonMovList = IsMovList(conts);
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsTrue(isJsonMovList, conts + " is not a valid movie list");
        }

        [TestMethod]
        public void UsersMoviesCurrentValidInUseId42NoMovs() {
            string umovs = users + "42/movies/current";
            var res = GetMessage(umovs);

            string conts = res.contents;
            HttpStatusCode status = res.status;

            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.AreEqual(conts, "[]"); //this it?
        }

        [TestMethod]
        public void UsersMoviesCurrentValidVacantIdTwo() {
            string umovs = users + "43/movies/current";
            var res = GetMessage(umovs);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void UsersMoviesCurrentValidVacantIdZero() {
            string umovs = users + "0/movies/current";
            var res = GetMessage(umovs);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void UsersMoviesCurrentInValidIdNegOne() {
            string umovs = users + "-1/movies/current";
            var res = GetMessage(umovs);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void UsersMoviesCurrentInValidIdNeg7k() {
            string umovs = users + "-70000/movies";
            var res = GetMessage(umovs);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void UsersMoviesCurrentInValidTypeTwenty() {
            string umovs = users + "twenty/movies/current";
            var res = GetMessage(umovs);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void UsersMoviesCurrentInValidTypeKatmandu() {
            string umovs = users + "katmandu/movies";
            var res = GetMessage(umovs);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        /*
         * moofytest/users/songs/
         */

        [TestMethod]
        public void UsersSongsValidInUseIdOne() {
            string usongs = users + "1/songs";
            var res = GetMessage(usongs);

            string conts = res.contents;
            HttpStatusCode status = res.status;

            bool isJsonSongList = IsSongList(conts);
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsTrue(isJsonSongList, conts + " is not a valid song list");
        }

        [TestMethod]
        public void UsersSongsValidInUseId7k() {
            string usongs = users + "70000/songs";
            var res = GetMessage(usongs);

            string conts = res.contents;
            HttpStatusCode status = res.status;

            bool isJsonSongList = IsSongList(conts);
            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.IsTrue(isJsonSongList, conts + " is not a valid song list");
        }

        [TestMethod]
        public void UsersSongsValidInUseNoMovs() {
            string usongs = users + "42/songs";
            var res = GetMessage(usongs);

            string conts = res.contents;
            HttpStatusCode status = res.status;

            Assert.AreEqual(HttpStatusCode.OK, status);
            Assert.AreEqual(conts, "[]"); //this it?
        }

        [TestMethod]
        public void UsersSongsValidVacantIdTwo() {
            string usongs = users + "2/songs";
            var res = GetMessage(usongs);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void UsersSongsInValidIdNegOne() {
            string usongs = users + "-1/songs";
            var res = GetMessage(usongs);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void UsersSongsInValidIdNeg7k() {
            string usongs = users + "-70000/songs";
            var res = GetMessage(usongs);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void UsersSongsInValidTypeTwenty() {
            string usongs = users + "twenty/songs";
            var res = GetMessage(usongs);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void UsersSongsInValidTypeKatmandu() {
            string usongs = users + "katmandu/songs";
            var res = GetMessage(usongs);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        /*
         * moofytest/songs
         */

        [TestMethod]
        public void SongsValidInUseIdOne() {
            string s = songs + "1";
            var res = GetMessage(s);

            string conts = res.contents;
            HttpStatusCode status = res.status;

            bool isJsonSong = IsSongList(conts);
            Assert.IsTrue(isJsonSong, conts + " is not a valid song");
            Assert.AreEqual(HttpStatusCode.OK, res.status); //assert 200
        }

        [TestMethod]
        public void SongValidInUseIdLarge() {
            string s = songs + "70000";
            var res = GetMessage(s);
            string conts = res.contents;

            bool isJsonSong = IsSong(conts);
            Assert.IsTrue(isJsonSong, conts + " is not a valid song");
            Assert.AreEqual(HttpStatusCode.OK, res.status); //assert 200
        }

        [TestMethod]
        public void SongsValidVacantId2() {
            string s = songs + "2";
            var res = GetMessage(s);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void SongsInValidIdZero() {
            string s = songs + "0";
            var res = GetMessage(s);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void SongsInValidIdNegOne() {
            string s = songs + "-1";
            var res = GetMessage(s);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void SongsInValidIdNeg7k() {
            string s = songs + "-70000";
            var res = GetMessage(s);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void SongsInValidTypeTwenty() {
            string s = songs + "twenty";
            var res = GetMessage(s);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void SongsInValidTypeKatmandu() {
            string s = songs + "Katmandu";
            var res = GetMessage(s);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        /*
        * moofytest/songs/download
        */

        [TestMethod]
        public void SongsDownloadValidInUseIdOne() {
            string s = songs + "1/download";
            var res = GetMessage(s);

            string conts = res.contents;
            HttpStatusCode status = res.status;

            bool isSongLnk = IsSongDownLink(conts);
            Assert.IsTrue(isSongLnk, conts + " is not a valid link");
            Assert.AreEqual(HttpStatusCode.OK, res.status); //assert 200
        }

        [TestMethod]
        public void SongsDownloadValidInUseIdLarge() {
            string s = songs + "70000/download";
            var res = GetMessage(s);
            string conts = res.contents;

            bool isSongLnk = IsSongDownLink(conts);
            Assert.IsTrue(isSongLnk, conts + " is not a valid link");
            Assert.AreEqual(HttpStatusCode.OK, res.status); //assert 200
        }

        [TestMethod]
        public void SongsDownloadValidVacantId2() {
            string s = songs + "2/download";
            var res = GetMessage(s);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void SongsDownloadInValidIdZero() {
            string s = songs + "0/download";
            var res = GetMessage(s);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void SongsDownloadInValidIdNegOne() {
            string s = songs + "-1/download";
            var res = GetMessage(s);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void SongsDownloadInValidIdNeg7k() {
            string s = songs + "-70000/download";
            var res = GetMessage(s);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void SongsDownloadInValidTypeTwenty() {
            string s = songs + "twenty/download";
            var res = GetMessage(s);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void SongsDownloadInValidTypeKatmandu() {
            string s = songs + "Katmandu/download";
            var res = GetMessage(s);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        /*
         * moofytest/songs/filter
         */

        [TestMethod]
        public void SongsFilter() {
            string s = songs + "filter/";
            var res = GetMessage(s);

            string conts = res.contents;
            HttpStatusCode status = res.status;

            bool isSongList = IsSongList(conts);
            //TODO doesn't test whether all movies are represented, not sure if it should either
            Assert.IsTrue(isSongList, conts + " is not a valid song list");
            Assert.AreEqual(HttpStatusCode.OK, res.status); //assert 200
        }

        /*
         * moofytest/songs/filter/{filter}
         */

        ///TODO  remove uselessness
        [TestMethod]
        public void SongsFilterFilter() {
            string s = movies + "filter/lars";
            var res = GetMessage(s);

            string conts = res.contents;
            HttpStatusCode status = res.status;

            bool isMovList = IsMovList(conts);
            Assert.IsTrue(isMovList, conts + " is not a valid song list");
            Assert.AreEqual(HttpStatusCode.OK, res.status); //assert 200
        }

        /*
         * moofytest/songs/filter/{filter}?sortBy={sortBy}
         */

        ///TODO  remove uselessness
        [TestMethod]
        public void SongsFilterSort() {
            string s = songs + "filter/lars?sortBy=title";
            var res = GetMessage(s);

            string conts = res.contents;
            HttpStatusCode status = res.status;

            bool isMovList = IsMovList(conts);
            Assert.IsTrue(isMovList, conts + " is not a valid song list");
            Assert.AreEqual(HttpStatusCode.OK, res.status); //assert 200
        }

        /*
        * moofytest/songs
        */

        [TestMethod]
        public void MoviesValidInUseIdOne() {
            string m = movies + "1";
            var res = GetMessage(m);

            string conts = res.contents;
            HttpStatusCode status = res.status;

            bool isJsonMov = IsMovie(conts);
            Assert.IsTrue(isJsonMov, conts + " is not a valid movie");
            Assert.AreEqual(HttpStatusCode.OK, res.status); //assert 200
        }

        [TestMethod]
        public void MoviesValidInUseIdLarge() {
            string m = movies + "70000";
            var res = GetMessage(m);
            string conts = res.contents;

            bool isJsonMov = IsMovie(conts);
            Assert.IsTrue(isJsonMov, conts + " is not a valid movie");
            Assert.AreEqual(HttpStatusCode.OK, res.status); //assert 200
        }

        [TestMethod]
        public void MoviesValidVacantId2() {
            string m = movies + "2";
            var res = GetMessage(m);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void MoviesInValidIdZero() {
            string m = movies + "0";
            var res = GetMessage(m);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void MoviesInValidIdNegOne() {
            string m = movies + "-1";
            var res = GetMessage(m);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void MoviesInValidIdNeg7k() {
            string m = movies + "-70000";
            var res = GetMessage(m);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void MoviesInValidTypeTwenty() {
            string m = movies + "twenty";
            var res = GetMessage(m);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void MoviesInValidTypeKatmandu() {
            string m = movies + "Katmandu";
            var res = GetMessage(m);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        /*
        * moofytest/movies/download
        */

        [TestMethod]
        public void MoviesDownloadValidInUseIdOne() {
            string m = movies + "1/download";
            var res = GetMessage(m);

            string conts = res.contents;
            HttpStatusCode status = res.status;

            bool isMovLnk = IsMovDownLink(conts);
            Assert.IsTrue(isMovLnk, conts + " is not a valid movie");
            Assert.AreEqual(HttpStatusCode.OK, res.status); //assert 200
        }

        [TestMethod]
        public void MoviesDownloadValidInUseIdLarge() {
            string m = movies + "70000/download";
            var res = GetMessage(m);
            string conts = res.contents;

            bool isMovLnk = IsMovDownLink(conts);
            Assert.IsTrue(isMovLnk, conts + " is not a valid movie");
            Assert.AreEqual(HttpStatusCode.OK, res.status); //assert 200
        }

        [TestMethod]
        public void MoviesDownloadValidVacantId2() {
            string m = movies + "2/download";
            var res = GetMessage(m);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void MoviesDownloadInValidIdZero() {
            string m = movies + "0/download";
            var res = GetMessage(m);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void MoviesDownloadInValidIdNegOne() {
            string m = movies + "-1/download";
            var res = GetMessage(m);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void MoviesDownloadInValidIdNeg7k() {
            string m = movies + "-70000/download";
            var res = GetMessage(m);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void MoviesDownloadInValidTypeTwenty() {
            string m = movies + "twenty/download";
            var res = GetMessage(m);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        [TestMethod]
        public void MoviesDownloadInValidTypeKatmandu() {
            string m = movies + "Katmandu/download";
            var res = GetMessage(m);

            HttpStatusCode status = res.status;
            Assert.AreEqual(HttpStatusCode.NotFound, status);
        }

        /*
         * moofytest/movies/filter
         */

        [TestMethod]
        public void MoviesFilter() {
            string m = movies + "filter/";
            var res = GetMessage(m);

            string conts = res.contents;
            HttpStatusCode status = res.status;

            bool isMovList = IsMovList(conts);
            //TODO doesn't test whether all movies are represented, not sure if it should either
            Assert.IsTrue(isMovList, conts + " is not a valid movie list");
            Assert.AreEqual(HttpStatusCode.OK, res.status); //assert 200
        }

        /*
         * moofytest/movies/filter/{filter}
         */

        ///TODO  remove uselessness
        [TestMethod]
        public void MoviesFilterFilter() {
            string m = movies + "filter/lars";
            var res = GetMessage(m);

            string conts = res.contents;
            HttpStatusCode status = res.status;

            bool isMovList = IsMovList(conts);
            Assert.IsTrue(isMovList, conts + " is not a valid movie list");
            Assert.AreEqual(HttpStatusCode.OK, res.status); //assert 200
        }

        /*
         * moofytest/movies/filter/{filter}?sortBy={sortBy}
         */

        ///TODO  remove uselessness
        [TestMethod]
        public void MoviesFilterSort() {
            string m = movies + "filter/lars?sortBy=title";
            var res = GetMessage(m);

            string conts = res.contents;
            HttpStatusCode status = res.status;

            bool isMovList = IsMovList(conts);
            Assert.IsTrue(isMovList, conts + " is not a valid movie list");
            Assert.AreEqual(HttpStatusCode.OK, res.status); //assert 200
        }
        /*
         * HELPERS
         */

        //Helper for receiving data and HTTP status code from url addr
        private Mess GetMessage(string addr) {
            Mess ret = new Mess();
            HttpWebResponse res=null;
                HttpWebRequest r = (HttpWebRequest)WebRequest.Create(addr);
                try{
                    res = (HttpWebResponse) r.GetResponse();
                } catch (WebException e) {

                }

            if (ret.status.Equals(HttpStatusCode.NotFound)) {
                ret.contents = "";
                return ret;
            }
            StreamReader reader = new StreamReader(res.GetResponseStream());
            String conts = reader.ReadToEnd();
            return new Mess { status = res.StatusCode, contents = conts };
        }

        //Helper for checking json data for songs
        private bool IsSong(string s) {
            return s.StartsWith("{") &&
                s.EndsWith("}") &&
                s.Contains("album") &&
                s.Contains("artist") &&
                s.Contains("genres") &&
                s.Contains("purchasePrice") &&
                s.Contains("release") &&
                s.Contains("rentalPrice") &&
                s.Contains("title");
        }

        //Helper for checking json data for songs
        private bool IsSongList(string s) {
            return s.StartsWith("[{") &&
                s.EndsWith("}]") &&
                s.Contains("album") &&
                s.Contains("artist") &&
                s.Contains("genres") &&
                s.Contains("purchasePrice") &&
                s.Contains("release") &&
                s.Contains("rentalPrice") &&
                s.Contains("title");
        }

        //Helper for checking json data for song downloadlink
        private bool IsSongDownLink(string sd) {
            return sd.StartsWith("{") &&
                sd.EndsWith("}") &&
                sd.Contains("downloadlink") &&
                sd.Contains("success") &&
                (sd.Contains("false") || sd.Contains("true"));
        }
        
        //Helper for checking json data for users
        private bool IsUser(string u){
            return u.StartsWith("{") &&
                u.EndsWith("}") &&
                u.Contains("balance") &&
                u.Contains("email") &&
                u.Contains("name") &&
                u.Contains("username");
        }

        //Helper for checking json data for movie
        private bool IsMovie(string ml) {
            return ml.StartsWith("{") &&
                ml.EndsWith("}") &&
                ml.Contains("description") &&
                ml.Contains("directors") &&
                ml.Contains("genres") &&
                ml.Contains("purchasePrice") &&
                ml.Contains("release") &&
                ml.Contains("rentalPrice") &&
                ml.Contains("title");
        }

        //Helper for checking json data for movielists
        private bool IsMovList(string ml) {
            return ml.StartsWith("[{") &&
                ml.EndsWith("}]") &&
                ml.Contains("description") &&
                ml.Contains("directors") &&
                ml.Contains("genres") &&
                ml.Contains("purchasePrice") &&
                ml.Contains("release") &&
                ml.Contains("rentalPrice") &&
                ml.Contains("title");
        }

        //Helper for checking json data for movie downloadlink
        private bool IsMovDownLink(string md) {
            return md.StartsWith("{") &&
                md.EndsWith("}") &&
                md.Contains("downloadlink") &&
                md.Contains("success") &&
                (md.Contains("false") || md.Contains("true"));
        }

        //Helper container for Http status code and response
        private class Mess {
            public HttpStatusCode status {
                get;
                set;
            }

            public string contents {
                get;
                set;
            }
        }
    }
}