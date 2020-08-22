using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MainServer.strategy
{
    public class Sync : IHttpRequest
    {
        private Quote quote;
        private UsersList users;
        private IConfiguration _iConfiguration;
        public Sync(IConfiguration _iConfiguration)
        {
            this.quote = new Quote();
            this.users = new UsersList();
            this._iConfiguration = _iConfiguration;
        }

        public Task<Response> Apply(string[] urls, string[] paths)
        {
            StringBuilder result = new StringBuilder();
            foreach(var path in paths)
            {

                string url = GetRandom(urls);
                result.Append(GetDataFromUrl(url, path)).Append(" ");

            }

            quote.GetQuoteFromString(result.ToString());


            return Task.Run(() => new Response(quote, users));

        }
        public string GetRandom(string[] arr)
         {
             Random random = new Random();
             int i = random.Next(1, arr.Length);
             return arr[i];
         }
        
        public string GetDataFromUrl(string url, string path)
         { 
             try
             {
                 return RequestToAnotherServer(url, path);
             }
             catch (WebException)
             {
                 return RequestToLocalServer(url, path);
             }
         }
         public string RequestToAnotherServer(string url, string path)
         {
             string word;
             User usr = new User();
             HttpWebRequest httpWebRequest = WebRequest.CreateHttp(url+path);
             
             using(WebResponse response = httpWebRequest.GetResponse())
             using(Stream dataStream = response.GetResponseStream())
             using(StreamReader reader = new StreamReader(dataStream))
             {

                 string userName = response.Headers.GetValues("InCamp-Student").First();
                 word = reader.ReadToEnd();

                 usr.Name = userName;
                 usr.Operation = word; 
                 users.AddUser(usr);
             }

             return usr.Operation;   
         }
          public string RequestToLocalServer(string url, string path)
         {

             DataStorage dataStorage = new DataStorage();
             
             User usr = new User();
             usr.Name = $"Shmyhol Roman, because {url} doesn't have response!";
             var s = dataStorage.GetQuoteElements().Where(arr => arr[0].Equals(path)).First();
             usr.Operation = GetRandom(s.ToArray()); 
             users.AddUser(usr);
            

             return usr.Operation;   
         }

    }
}