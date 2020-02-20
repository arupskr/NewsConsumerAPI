using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace HackerNewsConsumerAPI
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddMvc(options =>
            {
                options.Filters.Add(new ProducesAttribute("application/json"));
            });
            services.AddMvcCore()
                .AddJsonFormatters()
                .AddVersionedApiExplorer(
                    options =>
                    {
                        options.GroupNameFormat = "'v'VVV";
                        options.SubstituteApiVersionInUrl = true;
                    });
            ;
            services.AddResponseCaching();
            services.AddApiVersioning(options => options.ReportApiVersions = true);
            services.AddSwaggerGen(
                options =>
                {
                    var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerDoc(description.GroupName, new Info()
                        {
                            Title =
                                $"{this.GetType().Assembly.GetCustomAttribute<System.Reflection.AssemblyProductAttribute>().Product} {description.ApiVersion}",
                            Version = description.ApiVersion.ToString(),
                            Description = description.IsDeprecated
                                ? $"{this.GetType().Assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description} - DEPRECATED"
                                : this.GetType().Assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()
                                    .Description,

                        });
                    }
                    options.OperationFilter<SwaggerDefaultValues>();

                    options.IncludeXmlComments(Path.Combine(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), $"{this.GetType().Assembly.GetName().Name}.xml"
                    ));
                });

           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="provider"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseResponseCaching();
        }
    }
}
