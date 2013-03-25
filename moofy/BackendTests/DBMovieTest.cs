using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace moofy.Backend.Tests
{
    /// <summary>
    /// Summary description for DBMovieTest
    /// </summary>
    [TestClass]
    public class DBMovieTest
    {
        static DBAccess db;

        public DBMovieTest() {
            db = new DBAccess();
            db.Open();
        }

        [ClassCleanup()]
        public static void CleanUp() {
            db.Close();
        }

        private TestContext testContextInstance;

        [TestMethod]
        public void GetMovieTest()
        {
            //
            // TODO: Add test logic here
            //
        }
    }
}
