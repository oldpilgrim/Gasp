using HelloAspNet.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace HelloAspNet
{
    // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/?view=aspnetcore-5.0&tabs=windows
    // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup?view=aspnetcore-5.0
    // The Startup class is where:
    // Services required by the app are configured.
    // The app's request handling pipeline is defined, as a series of middleware components.
    
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // ASP.NET Core includes a built-in dependency injection (DI) framework that makes configured
        // services available throughout an app. For example, a logging component is a service.
        // Code to configure (or register) services is added to the Startup.ConfigureServices method.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "HelloAspNet", Version = "v1"});
            });
        }

        // The request handling pipeline is composed as a series of middleware components. Each component
        // performs operations on an HttpContext and either invokes the next middleware in the pipeline
        // or terminates the request.
        // By convention, a middleware component is added to the pipeline by invoking a Use... extension
        // method in the Startup.Configure method. For example, to enable rendering of static files,
        // call UseStaticFiles.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HelloAspNet v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}