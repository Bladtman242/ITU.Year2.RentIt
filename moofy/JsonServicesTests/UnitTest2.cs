using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.IO;

namespace moofy.JsonServices.Tests {
    [TestClass]
    public class UnitTest2 {
        [TestMethod]
        public void GetMessage() {
            string addr = "http://localhost:8732/moofytest/movies/upload/";
            HttpWebResponse res=null;
            HttpWebRequest r = (HttpWebRequest)WebRequest.Create(addr);
            r.Method = "POST";
            StreamWriter serverStream = new StreamWriter(r.GetRequestStream());
            serverStream.WriteLine("Lars og en elefant");


            try{
                res = (HttpWebResponse) r.GetResponse();
            } catch (WebException e) {}

            StreamReader reader = new StreamReader(res.GetResponseStream());
            String conts = reader.ReadToEnd();
            System.Diagnostics.Debug.WriteLine(conts);
        }
    }
}
