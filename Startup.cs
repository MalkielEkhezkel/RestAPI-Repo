using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApiTest.Data;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace WebApiTest
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
            services.AddControllers(options =>
            {
                options.RespectBrowserAcceptHeader = true; // false by default
            });

            //to get a command as XML and not json
            //services.AddControllers()
            //        .AddXmlSerializerFormatters();

            services.AddDbContext<WebApiTestContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("WebApiTestConnection")));
            //services.AddControllers();

            //this configuration is for patch request
            services.AddControllers().AddNewtonsoftJson(s=>
            {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            //here we are going to declare a using of data from repository. If I change the repository so
            //I can change the MockWebApiTestRepository and thats it because I already implemented 
            //an interface. 
            //there is 3 wais to do this
            //1. Singleton - Same for every request
            //2. Scoped - Creates once per client request
            //3. Transient - New instance created every time
            
            //services.AddScoped<IWebApiTestRepository, MockWebApiTestRepository>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IWebApiTestRepository, SqlWebApiTestRepository>();

            //for versioning with default parameters
            services.AddApiVersioning(config=>
            {
                // Specify the default API Version
                config.DefaultApiVersion = new ApiVersion(1, 0); 
                // If the client hasn't specified the API version in the request, use the default API version number  
                config.AssumeDefaultVersionWhenUnspecified = true;  
                // Advertise the API versions supported for the particular endpoint
                config.ReportApiVersions = true;  

                config.ApiVersionReader = new HeaderApiVersionReader("api-version");
                //config.ApiVersionReader = ApiVersionReader.Combine(new HeaderApiVersionReader("X-version"), new QueryStringApiVersionReader("api-version"));
            });

            services.AddSwaggerGen(config=> 
            {
                config.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "My WebApiTest project documentation", Version = "v1"});
            });

            //Deploy into IIS
            services.Configure<IISServerOptions>(options => 
            {
                options.AutomaticAuthentication = false;
            });
            services.Configure<IISOptions>(options => 
            {
                options.ForwardClientCertificate = false;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //for enabling a versioning
            app.UseApiVersioning();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(config=>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "My WebApiTest project documentation");
            });
        }
    }
}
