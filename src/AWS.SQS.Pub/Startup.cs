using Amazon.SQS;
using AWS.SQS.Pub.Configs;
using AWS.SQS.Pub.Helpers;
using AWS.SQS.Pub.Helpers.Interfaces;
using AWS.SQS.Pub.Services;
using AWS.SQS.Pub.Services.Interfaces;
using AWS.SQS.Pub.Workers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace AWS.SQS.Pub
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

            services.AddAWSService<IAmazonSQS>();

            services.AddTransient<IAWSSQSService, AWSSQSService>();
            services.AddTransient<IAWSSQSFifoService, AWSSQSFifoService>();

            services.AddTransient<IAWSSQSHelper, AWSSQSHelper>();
            services.AddTransient<IAWSSQSFifoHelper, AWSSQSFifoHelper>();

            services.AddHostedService<AWSSQSWorker>();
            services.AddHostedService<AWSSQSFifoAWorker>();
            services.AddHostedService<AWSSQSFifoBWorker>();


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
                endpoints.Map("/", async (ctx) =>
                {
                    await ctx.Response.WriteAsync("AWS SQS Api OK");
                });
            });
        }
    }
}
