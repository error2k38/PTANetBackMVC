using ApplicationServices.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationServices
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Services Configuration

            // Add services to the container.

            // Add HttpClient
            builder.Services.AddHttpClient();

            // Add controllers
            builder.Services.AddControllers(options =>
            {
                // Add a filter to validate model state
                options.Filters.Add(new ValidateModelAttribute());
            })
            .AddJsonOptions(options =>
            {
                // Configure JSON serializer to include null values
                options.JsonSerializerOptions.IgnoreNullValues = false;
            });

            // Configure API behavior options
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                // Suppress automatic 400 responses when model state is invalid
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            #endregion Services Configuration

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                    logger.LogError(exceptionHandlerPathFeature.Error, $"An unhanded exception occurred: {exceptionHandlerPathFeature.Error.StackTrace}");

                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("Server Internal Error");
                });
            });

            app.Run();
        }
    }
}
