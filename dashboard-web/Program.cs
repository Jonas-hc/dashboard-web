﻿using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Net;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Azure.Identity;
using Microsoft.Extensions.Configuration;

namespace dashboard_web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //string responseMessage = CallWebService("Kolding", "Jeger1studerende");
            //Console.WriteLine(responseMessage);
            CreateHostBuilder(args).Build().Run(); 
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .ConfigureAppConfiguration((ctx, builder) => {
                            var config = builder.Build();
                            Console.WriteLine(config["KeyVault:BaseUrl"]);
                            builder.AddAzureKeyVault(config["KeyVault:BaseUrl"]);
                        });
        });

        public static HttpWebRequest CreateWebRequest(string url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.ContentType = "text/xml; charset=utf-8";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        public static string CallWebService(String location, String key)
        {

            var _url = "https://smartweathersoap.azurewebsites.net/Forecast.asmx";

            XmlDocument soapEnvelopeXml = CreateSoapEnvelope(location, key);
            HttpWebRequest webRequest = CreateWebRequest(_url);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            // begin async call to web request.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            // suspend this thread until call is complete. You might want to
            // do something usefull here like update your UI.
            asyncResult.AsyncWaitHandle.WaitOne();

            // get the response from the completed web request.
            string soapResult;

            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                }

                string str = string.Format("<id>{0}", location);
                int startIdx = soapResult.IndexOf(str);
                string sub = soapResult.Substring(startIdx);
                int endIdx = sub.IndexOf("</location>");
                string newSub = sub.Substring(0, endIdx);
                string parentStart = "<forecast>";
                string parentEnd = "</forecast>";
                string wrappedString = parentStart + newSub;
                string newWrappedString = wrappedString + parentEnd;

                var a = newWrappedString.Split("<");
                string newXmlString = parentStart;

                foreach (var word in a)
                {
                    if (word.Contains("xsi"))
                    {
                        continue;
                    }
                    if (word.Contains("temp"))
                    {
                        newXmlString += "<" + word;
                    }
                    if (word.Contains("wind"))
                    {
                        newXmlString += "<" + word;
                    }
                    if (word.Contains("datetime"))
                    {
                        newXmlString += "<" + word;
                    }
                }

                newXmlString += parentEnd;

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(newXmlString);
                string jsonText = JsonConvert.SerializeXmlNode(doc);
                var jo = JObject.Parse(jsonText);
                var temp = jo["forecast"]["temp"];
                var windchill = jo["forecast"]["windchill"];
                var weather1 = jo["forecast"]["datetime"];


                Weather weather = new Weather((double)temp, (double)windchill, (string)weather1);

                string weatherObj = JsonConvert.SerializeObject(weather);
                return weatherObj;

            }
        }

        public static XmlDocument CreateSoapEnvelope(string location, string key)
        {
            XmlDocument soapEnvelopeDocument = new XmlDocument();
            Console.WriteLine(" -- Jonas --");
            Console.WriteLine(key);

            String s = String.Format("" +
                @"<?xml version=""1.0"" encoding=""utf-8""?>
            <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                <soap:Body>
                    <GetForecast xmlns=""http://tempuri.org/"">
                        <key>{0}</key>
                        <location>{1}</location>
                    </GetForecast>
                </soap:Body>
            </soap:Envelope>
            ", key, location);
            soapEnvelopeDocument.LoadXml(s);
            return soapEnvelopeDocument;
        }

        public static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }

        public class Weather
        {
            public double Temp;
            public double Windchill;
            public string DateAndTime;

            public Weather(double temp, double wc, string dt)
            {
                Temp = temp;
                Windchill = wc;
                DateAndTime = dt;
            }
        }
    }
}