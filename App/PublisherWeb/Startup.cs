using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

//using System.Net.Http;
using Microsoft.AspNetCore.Http;

using PublisherWeb.ClientApi;
using PublisherWeb.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
namespace PublisherWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //register delegating handlers
            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();
            //  services.AddTransient<HttpClientRequestIdDelegatingHandler>();

            //set 5 min as the lifetime for each HttpMessageHandler int the pool
            services.AddHttpClient("extendedhandlerlifetime").SetHandlerLifetime(TimeSpan.FromMinutes(5));

            //add http client services
            services.AddHttpClient<TodoItemClient, TodoItemClient>()
                   .SetHandlerLifetime(TimeSpan.FromMinutes(5))  //Sample. Default lifetime is 2 minutes
                   .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();

            // Set Default Authentication Name 
            services.AddAuthentication(option =>
            {
                option.DefaultScheme = "Cookies";
                option.DefaultChallengeScheme = "oidc";
            }).AddCookie("Cookies")// Add Authentication Method by Cookies
            .AddOpenIdConnect("oidc",options =>
            {
                options.Authority = "http://localhost:5000";
                options.ClientId = "publisher-web.wwf.com";
                options.ClientSecret = "password";
                options.ResponseType = "code";
                options.CallbackPath="/sign-oidc";
               // options.SignedOutCallbackPath="/TestAfterLogout"; => this Property does not work.
                options.RequireHttpsMetadata = false;
                options.SaveTokens = true;
                options.Scope.Add("organizer");
            }); // Add Authentication Method by OpenIdConnect

            //options =>{
       // options.ForwardChallenge ="oidc";}

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
        //    app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
