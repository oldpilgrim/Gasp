using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Congo.Models;
using Congo.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace Congo
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
            // The configuration instance to which the appsettings.json file's BookstoreDatabaseSettings section binds
            // is registered in the Dependency Injection (DI) container.
            services.Configure<BookstoreDatabaseSettings>(
                Configuration.GetSection(nameof(BookstoreDatabaseSettings)));
            
            // The IBookstoreDatabaseSettings interface is registered in DI with a singleton service lifetime.
            // When injected, the interface instance resolves to a BookstoreDatabaseSettings object.
            services.AddSingleton<IBookstoreDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<BookstoreDatabaseSettings>>().Value);
            
            // the BookService class is registered with DI to support constructor injection in consuming classes.
            // The singleton service lifetime is most appropriate because BookService takes a direct dependency
            // on MongoClient. Per the official Mongo Client reuse guidelines,
            // MongoClient should be registered in DI with a singleton service lifetime.
            services.AddSingleton<BookService>();

            // Make property names in the web API's serialized JSON response match their corresponding property names in the CLR object type
            // For example, the Book class's Author property serializes as Author.
            services.AddControllers().AddNewtonsoftJson(options => options.UseMemberCasing());
            
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Congo", Version = "v1"}); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Congo v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}