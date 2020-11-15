using System;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;

namespace Countries
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
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            SetupDatabase();
        }

        public void SetupDatabase()
        {
            Thread.Sleep(10000);

            const string server = "10.0.10.3";
            const string port = "3306";
            const string uid = "root";
            const string password = "admin";
            const string database = "countries";

            for (int i = 0; i < 20; i++)
            {
                try
                {
                    using var connection = new MySqlConnection($"server={server};port={port};uid={uid};pwd={password}");
                    connection.Open();

                    var command = new MySqlCommand($"create database if not exists `{database}`", connection);

                    command.ExecuteNonQuery();
                    connection.Close();

                    break;
                }
                catch (Exception)
                {
                    if (i == 19)
                    {
                        throw;
                    }
                }

                MySqlConnection.ClearAllPools();

                Thread.Sleep(5000);
            }
        }
    }
}
