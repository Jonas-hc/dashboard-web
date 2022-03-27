using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace dashboard_web
{
    public class GetOutput
    {
        public GetOutput()
        {
        }

        public int Getoutput(string username, string pw, string fileName)
        {
            string url = string.Format("ftp://inverter.westeurope.cloudapp.azure.com/{0}", fileName);
            FtpWebRequest request1 = (FtpWebRequest)WebRequest.Create(url);
            request1.Method = WebRequestMethods.Ftp.DownloadFile;

            string user = username;
            string password = pw;

            request1.Credentials = new NetworkCredential(user, password);

            FtpWebResponse response1 = (FtpWebResponse)request1.GetResponse();
            var stream1 = response1.GetResponseStream();
            var reader1 = new StreamReader(stream1);

            List<string> strContent = new List<string>();

            while (!reader1.EndOfStream)
            {
                strContent.Add(reader1.ReadLine());
            }

            int hourEnd = 0;
            int hourStart = 0;
            Boolean unlock = false;
            List<string> outPut = new List<string>();


            foreach (var line in strContent)
            {
                var values = line.Split(";");
                if (line.Contains("[wr_ende]"))
                {
                    unlock = false;
                }

                if (unlock)
                {
                    outPut.Add($"{values[38]}");
                }

                if (line.Contains("INTERVAL;"))
                {
                    unlock = true;
                }
            }

            int lastIdx = outPut.Count;
            hourStart = Int32.Parse(outPut[0]);
            hourEnd = Int32.Parse(outPut[lastIdx - 1]);
            int sum = hourEnd - hourStart;

            reader1.Close();
            response1.Close();
            return sum;
        }
    }
}
