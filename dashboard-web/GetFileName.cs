using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace dashboard_web
{
    public class GetFileName
    {
        public GetFileName()
        {
        }
        public string GetFileNameMethod(string username, string pw)
        {
            string url = string.Format("ftp://inverter.westeurope.cloudapp.azure.com");
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            string user = username;
            string password = pw;

            request.Credentials = new NetworkCredential(user, password);

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            var stream = response.GetResponseStream();
            var reader = new StreamReader(stream);


            List<string> strContent = new List<string>();
            while (!reader.EndOfStream)
            {
                var a = reader.ReadLine();
                Console.WriteLine(a);
                Console.WriteLine(" ");
                strContent.Add(a);
            }
            var item = "";
            if (strContent.Count > 0)
            {
                item = strContent[^1];
            }
            reader.Close();
            response.Close();
            return item;
        }
    }
}
