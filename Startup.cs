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
            services.AddControllers();

            services.AddScoped<IPaisesService, PaisesService>();
            services.AddScoped<IBusquedaService, BusquedaService>();

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
