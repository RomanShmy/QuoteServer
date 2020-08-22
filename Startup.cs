
using System.Net.Sockets;
using System.Net;
using System.Text;

using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MainServer.strategy;
using Microsoft.Extensions.Configuration;

namespace MainServer
{
    public class Startup
    {
        private IHttpRequest request;
        private Profiler profiler;
        
        
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
            profiler = new Profiler();
            try
            {
                request = GetHttpRequest(configuration.GetValue<string>("arg"));
                profiler.AddType(configuration.GetValue<string>("arg"));
            }
            catch
            {
                request = GetHttpRequest();
                profiler.AddType();
            }
            
        }

        private IConfiguration Configuration { get; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) { }

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
                string[] paths = {"who", "how", "does", "what"};
                string[] urls = new string[11]; 
                urls[0] = "";
                for(int i = 1; i <= Int32.Parse(Environment.GetEnvironmentVariable("count").ToString()); i++)
                {
                    urls[i] = $"http://mainserver_service_{i}/";
                }
                DataStorage storage = new DataStorage();
                
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello world!");
                });
                endpoints.MapGet("/who", async context =>
                {
                    
                    string result = GetRandom(storage.Who);
                    
                    context.Response.ContentType = "text/html; charset=utf-8";
                    context.Response.Headers.Add("InCamp-Student", Dns.GetHostName());
                    await context.Response.WriteAsync(result);
                });
                endpoints.MapGet("/how", async context =>
                {
                    string result = GetRandom(storage.How);
                    
                    context.Response.ContentType = "text/html; charset=utf-8";
                    context.Response.Headers.Add("InCamp-Student", Dns.GetHostName());
                    await context.Response.WriteAsync(result);
                });
                endpoints.MapGet("/does", async context =>
                {
                    string result = GetRandom(storage.Does);
                    context.Response.Headers.Add("InCamp-Student", Dns.GetHostName());
                    context.Response.ContentType = "text/html; charset=utf-8";
                    await context.Response.WriteAsync(result);
                });
                endpoints.MapGet("/what", async context =>
                {
                    string result = GetRandom(storage.What);
                    context.Response.Headers.Add("InCamp-Student", Dns.GetHostName());
                    context.Response.ContentType = "text/html; charset=utf-8";
                    
                    await context.Response.WriteAsync(result);
                });
                endpoints.MapGet("/quote", async context =>
                {
                    List<string[]> qouteElements = storage.GetQuoteElements();
                    StringBuilder result = new StringBuilder();
                    

                    foreach(var qouteElement in qouteElements)
                    {
                        result.Append(GetRandom(qouteElement)).Append(" ");
                    }
                    
                    context.Response.ContentType = "text/html; charset=utf-8";
                    await context.Response.WriteAsync(result.ToString());
                });

                endpoints.MapGet("/incamp18-quote", async context =>
                {    
                    DateTime start = DateTime.Now;
                    
                    Task<Response> response = request.Apply(urls, paths);
                    DateTime finish = DateTime.Now;
                    var time = finish - start;
                    profiler.AddTime(time);

                    context.Response.ContentType = "text/html; charset=utf8";
                    await context.Response.WriteAsync(response.Result.GetResponse() + "<br>\n" + profiler.GetProccessTime());
                    
                    
                    response.Result.Clear();
                });
                
                
            });
        }
        public string GetRandom(string[] arr)
         {
             Random random = new Random();
             int i = random.Next(1, arr.Length);
             return arr[i];
         }

         public IHttpRequest GetHttpRequest(string arg = "sync")
         {
             if(arg.Equals("async"))
             {
                 return new Async();
             }

             return new Sync(Configuration);
         }
    }
}
