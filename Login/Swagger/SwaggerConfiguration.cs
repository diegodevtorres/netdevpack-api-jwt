using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;

namespace Login.Swagger
{
    public static class SwaggerConfiguration
    {
        public static void Config(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Login",
                    Version = "v1",
                    Description = "API de Login",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Login",
                        Email = "diegodevtorres@gmail.com.br",
                        Url = new Uri("http://www.diegodevtorres.com.br")
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
        }
    }
}
