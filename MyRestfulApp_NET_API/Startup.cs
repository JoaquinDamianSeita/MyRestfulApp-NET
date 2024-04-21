using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using MyRestfulApp_NET_API.Common;
using MyRestfulApp_NET_API.Common.Middlewares;
using MyRestfulApp_NET_API.Domain.HttpClients;
using MyRestfulApp_NET_API.Domain.Repositories;
using MyRestfulApp_NET_API.Domain.Services;
using MyRestfulApp_NET_API.Persistence.Contexts;
using MyRestfulApp_NET_API.Persistence.Repositories;
using MyRestfulApp_NET_API.Services;

namespace MyRestfulApp_NET_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("MyRestfulApp"),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 10,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    });
            });

            services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>()).ConfigureApiBehaviorOptions(options => {
                options.InvalidModelStateResponseFactory = c => {
                    var errors = string.Join('\n', c.ModelState.Values.Where(v => v.Errors.Count > 0)
                        .SelectMany(v => v.Errors)
                        .Select(v => v.ErrorMessage));

                    return new ValidationErrorResponse { Message = errors };
                };
            });

            services.AddScoped<IPaisesService, PaisesService>();
            services.AddScoped<IBusquedaService, BusquedaService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICurrencyService, CurrencyService>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddHttpClient<IMercadoLibreClient, MercadoLibreClient>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "MyRestfulApp", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // In development environment, show detailed error information
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // In other environments, handle errors more friendly
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            // Add middleware to handle custom exceptions
            app.UseMiddleware<CustomExceptionHandlerMiddleware>();

            // Add middleware to redirect HTTP to HTTPS
            app.UseHttpsRedirection();

            // Add middleware to serve static files (for example, HTML, CSS, images)
            app.UseStaticFiles();

            // Add middleware for routing and endpoints
            app.UseRouting();

            // Add Swagger UI to the request pipeline
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            // Add middleware to run the endpoint of the request
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
