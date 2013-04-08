using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.IO;
using System.Text;

namespace moofy.JsonServices.Tests {
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void GetMessage()
        {
            string addr = "http://localhost:8732/moofytest/movies/upload";
            HttpWebResponse res = null;
            HttpWebRequest r = (HttpWebRequest)WebRequest.Create(addr);
            r.Method = "POST";
            StreamWriter serverStream = new StreamWriter(r.GetRequestStream());
            serverStream.WriteLine("Lars og en elefant");


            try
            {
                res = (HttpWebResponse)r.GetResponse();
            }
            catch (WebException e) { res = e.Response as HttpWebResponse; }

            StreamReader reader = new StreamReader(res.GetResponseStream());
            String conts = reader.ReadToEnd();
            System.Diagnostics.Debug.WriteLine(conts);
        }
        [TestMethod]
        public void roflpops(){
            HttpWebRequest httpWReq =
                (HttpWebRequest)WebRequest.Create(@"http://127.0.0.1:8732/moofytest/movies/upload");//"http://rentit.itu.dk/RentIt25/moofy.svc/movies/upload");

            FileStream fs = new FileStream("C:/RENTIT25/is.txt", FileMode.Open);
            var memoryStream = new MemoryStream();
            fs.CopyTo(memoryStream);
            byte[] data =  memoryStream.ToArray();

            httpWReq.Method = "POST";
            httpWReq.ContentType = "application/x-www-form-urlencoded";//Possibly "application/json; charset=utf-8" instead
            httpWReq.ContentLength = data.Length;

            using (Stream stream = httpWReq.GetRequestStream())
            {
                stream.Write(data,0,data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();

            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            System.Diagnostics.Debug.WriteLine(responseString);
        }
    }

}
