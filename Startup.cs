using System.Text;
using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;

namespace MainServer
{
    public class Startup
    {
        private UsersList users;

        public Startup()
        {
            users = new UsersList();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                Quote quote = new Quote();
                string[] who = {"Барсик", "Мой друг", "Собака"};
                string[] how = {"красиво", "глупо", "плохо"};
                string[] does = {"пишет", "рисует", "танцует"};
                string[] what = {"код", "танго", "море"};

                
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
                endpoints.MapGet("/who", async context =>
                {
                    
                    string result = GetRandomWord(who);
                    quote.Who = result;
                    context.Response.ContentType = "text/html; charset=utf-8";
                    context.Response.Headers.Add("InCamp-Student", "Shmyhol Roman");
                    await context.Response.WriteAsync(result);
                });
                endpoints.MapGet("/how", async context =>
                {
                    string result = GetRandomWord(how);
                    quote.How = result;
                    context.Response.ContentType = "text/html; charset=utf-8";
                    context.Response.Headers.Add("InCamp-Student", "Shmyhol Roman");
                    await context.Response.WriteAsync(result);
                });
                endpoints.MapGet("/does", async context =>
                {
                    string result = GetRandomWord(does);
                    quote.Does = result; 
                    context.Response.ContentType = "text/html; charset=utf-8";
                    context.Response.Headers.Add("InCamp-Student", "Shmyhol Roman");
                    await context.Response.WriteAsync(result);
                });
                endpoints.MapGet("/what", async context =>
                {
                    string result = GetRandomWord(what);
                    quote.What = result;
                    context.Response.ContentType = "text/html; charset=utf-8";
                    context.Response.Headers.Add("InCamp-Student", "Shmyhol Roman");
                    await context.Response.WriteAsync(result);
                });

                endpoints.MapGet("/quote", async context =>
                {
                    context.Response.ContentType = "text/html; charset=utf-8";
                    await context.Response.WriteAsync(quote.ToString());
                });

                endpoints.MapGet("/incamp18-quote", async context =>
                {
                    string[] paths = {"who", "how", "does", "what"}; 
                    string[] urls = {"http://localhost:1234/",
                                     "http://localhost:56555/",  
                                     "http://546906f46143.ngrok.io/", 
                                     "http://a88b942d765c.ngrok.io/", 
                                     "http://ab95f7fa1783.ngrok.io/", 
                                     "http://72264496c037.ngrok.io/", 
                                     "http://81b49b859709.ngrok.io/", 
                                     "http://b25753c8cc31.ngrok.io/", 
                                     "http://5788514b9e11.ngrok.io/"
                                    };
                    StringBuilder result = new StringBuilder();
                    DateTime start = DateTime.Now;
                   
                    Task<string> who = GetDataFromUrl(GetRandomUrl(urls), paths[0]);
                    Task<string> how = GetDataFromUrl(GetRandomUrl(urls), paths[1]);
                    Task<string> does = GetDataFromUrl(GetRandomUrl(urls), paths[2]);
                    Task<string> what = GetDataFromUrl(GetRandomUrl(urls), paths[3]);

                    quote.Who = await who;
                    quote.How = await how;
                    quote.Does = await does;
                    quote.What = await what;
                    
                    // foreach(var path in paths)
                    // {
                        
                    //     string url = GetRandomUrl(urls);
                    //     result.Append(GetDataFromUrl(url, path)).Append(" ");
                        
                    // }
                    
                    DateTime finish = DateTime.Now;
                    Console.WriteLine(finish - start);
                    context.Response.Headers.Add("InCamp-Student", "Shmyhol Roman");
                    context.Response.ContentType = "text/html; charset=utf8";
                    
                    await context.Response.WriteAsync(quote.ToString() + "<br>" + users.GetUserOperationList());
                    users.Clear();
                   
                });
                
            });
        }

        public string GetRandomWord(string[] words)
        {
            Random random = new Random();
            int i = random.Next(0, words.Length);
            return words[i];
        }
        public string GetRandomUrl(string[] ips)
        {
            Random rm = new Random();
            int i = rm.Next(0, ips.Length);
            return ips[i];
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
            string word;
            User usr = new User();

            HttpWebRequest httpWebRequest = WebRequest.CreateHttp(url+path);

            using(WebResponse response = await httpWebRequest.GetResponseAsync())
            using(Stream dataStream = response.GetResponseStream())
            using(StreamReader reader = new StreamReader(dataStream))
            {
                
                string userName = response.Headers.GetValues("InCamp-Student").First();
                word = await reader.ReadToEndAsync();
                
                usr.Name = userName;
                usr.Operation = word; 
                users.AddUser(usr);
            }

            return await Task.Run(()=>usr.Operation);   
        }
         public async Task<string> RequestToLocalServer(string url, string path)
        {
            const string mainUrl = "http://localhost:56555/";
            string word;
            User usr = new User();

            HttpWebRequest httpWebRequest = WebRequest.CreateHttp(mainUrl+path);

            using(WebResponse response = await httpWebRequest.GetResponseAsync())
            using(Stream dataStream = response.GetResponseStream())
            using(StreamReader reader = new StreamReader(dataStream))
            {
                
                string userName = response.Headers.GetValues("InCamp-Student").First();
                word = await reader.ReadToEndAsync();
                
                usr.Name = $"{userName}, because {url} doesn't have response!";
                usr.Operation = word; 
                users.AddUser(usr);
            }

            return await Task.Run(()=>usr.Operation);   
        }
        

        
        // public string GetDataFromUrl(string url, string path)
        // { 
        //     try
        //     {
        //         return RequestToAnotherServer(url, path);
        //     }
        //     catch (WebException)
        //     {
        //         return RequestToLocalServer(url, path);
        //     }
        // }
        // public string RequestToAnotherServer(string url, string path)
        // {
        //     string word;
        //     User usr = new User();

        //     HttpWebRequest httpWebRequest = WebRequest.CreateHttp(url+path);

        //     using(WebResponse response = httpWebRequest.GetResponse())
        //     using(Stream dataStream = response.GetResponseStream())
        //     using(StreamReader reader = new StreamReader(dataStream))
        //     {
                
        //         string userName = response.Headers.GetValues("InCamp-Student").First();
        //         word = reader.ReadToEnd();
                
        //         usr.Name = userName;
        //         usr.Operation = word; 
        //         users.AddUser(usr);
        //     }

        //     return usr.Operation;   
        // }
        //  public string RequestToLocalServer(string url, string path)
        // {
        //     const string mainUrl = "http://localhost:56555/";
        //     string word;
        //     User usr = new User();

        //     HttpWebRequest httpWebRequest = WebRequest.CreateHttp(mainUrl+path);

        //     using(WebResponse response = httpWebRequest.GetResponse())
        //     using(Stream dataStream = response.GetResponseStream())
        //     using(StreamReader reader = new StreamReader(dataStream))
        //     {
                
        //         string userName = response.Headers.GetValues("InCamp-Student").First();
        //         word = reader.ReadToEnd();
                
        //         usr.Name = $"{userName}, because {url} doesn't have response!";
        //         usr.Operation = word; 
        //         users.AddUser(usr);
        //     }

        //     return usr.Operation;   
        // }
        
    }
}
