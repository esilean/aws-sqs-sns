using Amazon.SimpleNotificationService;
using AWS.SNS.Pub.Configs;
using AWS.SNS.Pub.Services;
using AWS.SNS.Pub.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace AWS.SNS.Pub
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

            services.Configure<ServiceConfiguration>(Configuration.GetSection("ServiceConfiguration"));

            services.AddAWSService<IAmazonSimpleNotificationService>();

            services.AddScoped<IAWSSNSService, AWSSNSService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AWS API", Version = "v1" });
            });
            services.AddCors(options =>
            {
                options.AddPolicy(name: "AWS-Policy", builder =>
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AWS API V1");
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("AWS-Policy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.Map("/", async (ctx) =>
                //{
                //    await ctx.Response.WriteAsync("AWS SNS Api OK");
                //});
            });
        }
    }
}
