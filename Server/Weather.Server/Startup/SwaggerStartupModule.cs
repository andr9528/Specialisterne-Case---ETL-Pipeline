using Microsoft.OpenApi;
using Weather.Abstraction.Interfaces.Startup;

namespace Weather.Server.Startup
{
    public class SwaggerStartupModule : IServiceStartupModule, IApplicationStartupModule<IApplicationBuilder>
    {
        private readonly string apiTitle;

        public SwaggerStartupModule(string apiTitle)
        {
            this.apiTitle = apiTitle;
        }

        /// <inheritdoc />
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = apiTitle, Version = "v1",}); });
            services.AddSwaggerGenNewtonsoftSupport();
        }

        /// <inheritdoc />
        public void ConfigureApplication(IApplicationBuilder app)
        {
            if (app is not WebApplication webApplication)
                throw new InvalidOperationException(
                    $"Expected Supplied App to be of type {nameof(WebApplication)}, but it was a {app.GetType().Name}.");

            if (webApplication.Environment.IsDevelopment())
            {
                webApplication.UseSwagger();
                webApplication.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{apiTitle}");
                    c.RoutePrefix = string.Empty;
                });
            }
        }
    }
}