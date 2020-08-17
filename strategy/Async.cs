using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MainServer.strategy
{
    public class Async : IHttpRequest
    {
        private Quote quote;
        private UsersList users;

        public Async()
        {
            this.quote = new Quote();
            this.users = new UsersList();
        }

        public async Task<Response> Apply(string[] urls, string[] paths)
        {
            Task<string> who = GetDataFromUrl(GetRandom(urls), paths[0]);
            Task<string> how = GetDataFromUrl(GetRandom(urls), paths[1]);
            Task<string> does = GetDataFromUrl(GetRandom(urls), paths[2]);
            Task<string> what = GetDataFromUrl(GetRandom(urls), paths[3]);
                
            quote.Who = await who;
            quote.How = await how;
            quote.Does = await does;
            quote.What = await what;        


            return new Response(quote, users);

        }
        public string GetRandom(string[] arr)
         {
             Random random = new Random();
             int i = random.Next(0, arr.Length);
             return arr[i];
         }
        
        public async Task<string> GetDataFromUrl(string url, string path)
        { 
            try
            {
                return await RequestToAnotherServer(url, path);
            }
            catch (WebException)
            {
                return await RequestToLocalServer(url, path);
            }
        }
        public async Task<string> RequestToAnotherServer(string url, string path)
        {
            string word, userName;
            User usr = new User();

            HttpWebRequest httpWebRequest = WebRequest.CreateHttp(url+path);

            using(WebResponse response = await httpWebRequest.GetResponseAsync())
            using(Stream dataStream = response.GetResponseStream())
            using(StreamReader reader = new StreamReader(dataStream))
            {
                userName = response.Headers.GetValues("InCamp-Student").First();
                word = reader.ReadToEnd();
            }

            usr.Name = userName;
            usr.Operation = word; 
            users.AddUser(usr);

            return usr.Operation;   
        }
         public async Task<string> RequestToLocalServer(string url, string path)
        {
            const string mainUrl = "http://localhost:56555/";
            string word, userName;
            User usr = new User();

            WebRequest httpWebRequest = WebRequest.Create(mainUrl+path);

            using(WebResponse response = await httpWebRequest.GetResponseAsync())
            using(Stream dataStream = response.GetResponseStream())
            using(StreamReader reader = new StreamReader(dataStream))
            {
                userName = response.Headers.GetValues("InCamp-Student").First();
                word = reader.ReadToEnd();
            }

            usr.Name = $"{userName}, because {url} doesn't have response!";
            usr.Operation = word; 
            users.AddUser(usr);

            return usr.Operation;   
        }

    }
}