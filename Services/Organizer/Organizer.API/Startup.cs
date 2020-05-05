using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


using wwf.Services.Organizer.API.Data;
using Microsoft.EntityFrameworkCore;
namespace Organizer.API
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
            services.AddControllers();

            // SetDbContext
            services.AddDbContext<TodoContext>(option => { option.UseInMemoryDatabase("TodoDB"); });

            // add OpenApi
            services.AddOpenApiDocument();

            //add JwtBearer as Default Authentication
            services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", option =>
            {
                option.Authority = "http://localhost:5000";
                option.Audience = "organizer";
                option.RequireHttpsMetadata = false;
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute(); // for support ViewEngine of MVC
            });
        }
    }
}
