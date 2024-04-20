using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using MyRestfulApp_NET.Common;
using MyRestfulApp_NET.Common.Middlewares;
using MyRestfulApp_NET.Domain.Repositories;
using MyRestfulApp_NET.Domain.Services;
using MyRestfulApp_NET.Persistence.Contexts;
using MyRestfulApp_NET.Persistence.Repositories;
using MyRestfulApp_NET.Services;

namespace MyRestfulApp_NET
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
                // En entorno de desarrollo, mostrar información detallada sobre errores
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // En otros entornos, manejar errores de manera más amigable
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            // Agregar middleware para manejar excepciones personalizadas
            app.UseMiddleware<CustomExceptionHandlerMiddleware>();

            // Agregar middleware para redirigir HTTP a HTTPS
            app.UseHttpsRedirection();

            // Agregar middleware para servir archivos estáticos (por ejemplo, HTML, CSS, imágenes)
            app.UseStaticFiles();

            // Agregar middleware para enrutamiento y endpoints
            app.UseRouting();

            // Agregar Swagger UI al pipeline de solicitud
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            // Middleware para autenticación, autorización, etc. se agrega aquí si es necesario

            // Agregar middleware para ejecutar el endpoint de la solicitud
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // Configurar enrutamiento para los controllers
            });
        }
    }
}
