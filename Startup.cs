using System.Runtime.Intrinsics.X86;
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
using System.Threading;
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
            Configuration = configuration;
            profiler = new Profiler();
            try
            {
                request = GetHttpRequest(configuration.GetValue<string>("arg"));
            }
            catch
            {
                request = GetHttpRequest();
            }
            
        }

        private IConfiguration Configuration { get; }
        
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
                
                string[] paths = {"who", "how", "does", "what"}; 
                string[] who = {"Барсик", "Мой друг", "Собака"};
                string[] how = {"красиво", "глупо", "плохо"};
                string[] does = {"пишет", "рисует", "танцует"};
                string[] what = {"код", "танго", "море"};
                string[] urls = {"http://d0165c8e5358.ngrok.io/",
                                     "http://12f1a14e7e50.ngrok.io/",
                                     "http://4c1449a93861.ngrok.io/",
                                     "http://7a45a5f78857.ngrok.io/",
                                     "http://e77fd3b7ed59.ngrok.io/",
                                     "http://a089177a583a.ngrok.io/",
                                     "http://aba617d86eae.ngrok.io/",
                                     "http://26b139b05b0f.ngrok.io/",
                                     "http://17f7ddd05769.ngrok.io/",
                                     "http://5e9e572e07b3.ngrok.io/",
                                     "https://8a2f59ef9085.ngrok.io/",
                                     "http://67e5aa89deb6.ngrok.io/"
                                    };
                
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
                endpoints.MapGet("/who", async context =>
                {
                    
                    string result = GetRandom(who);
                    
                    context.Response.ContentType = "text/html; charset=utf-8";
                    context.Response.Headers.Add("InCamp-Student", "Shmyhol Roman");
                    await context.Response.WriteAsync(result);
                });
                endpoints.MapGet("/how", async context =>
                {
                    string result = GetRandom(how);
                    
                    context.Response.ContentType = "text/html; charset=utf-8";
                    context.Response.Headers.Add("InCamp-Student", "Shmyhol Roman");
                    await context.Response.WriteAsync(result);
                });
                endpoints.MapGet("/does", async context =>
                {
                    string result = GetRandom(does);
                    
                    context.Response.ContentType = "text/html; charset=utf-8";
                    context.Response.Headers.Add("InCamp-Student", "Shmyhol Roman");
                    await context.Response.WriteAsync(result);
                });
                endpoints.MapGet("/what", async context =>
                {
                    string result = GetRandom(what);
                    
                    context.Response.ContentType = "text/html; charset=utf-8";
                    context.Response.Headers.Add("InCamp-Student", "Shmyhol Roman");
                    await context.Response.WriteAsync(result);
                });

                endpoints.MapGet("/quote", async context =>
                {
                    context.Response.ContentType = "text/html; charset=utf-8";
                    StringBuilder result = new StringBuilder();
                    foreach(var path in paths)
                    {
                        
                    }
                    await context.Response.WriteAsync(result.ToString());
                });

                endpoints.MapGet("/incamp18-quote", async context =>
                {
                    
                    DateTime start = DateTime.Now;
                    context.Response.Headers.Add("InCamp-Student", "Shmyhol Roman");
                    context.Response.ContentType = "text/html; charset=utf8";
                    Task<Response> response = request.Apply(urls, paths);
                    DateTime finish = DateTime.Now;
                    var time = finish - start;
                    profiler.AddTime(time);
                    await context.Response.WriteAsync(response.Result.GetResponse() + "<br>\n" + profiler.GetProccessTime());
                    
                    
                    response.Result.Clear();
                });
                
            });
        }
        public string GetRandom(string[] arr)
         {
             Random random = new Random();
             int i = random.Next(0, arr.Length);
             return arr[i];
         }

         public IHttpRequest GetHttpRequest(string arg = "sync")
         {
             if(arg.Equals("async"))
             {
                 return new Async();
             }

             return new Sync();
         }

        
        
    }
}
