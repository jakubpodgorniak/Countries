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

            const string database = "countries";

            for (int i = 0; i < 20; i++)
            {
                try
                {
                    using var connection = new MySqlConnection(Configuration.GetConnectionString("Root"));
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

            CreateCountryTable();
        }

        private void CreateCountryTable()
        {
            using var connection = new MySqlConnection(Configuration.GetConnectionString("Db"));
            connection.Open();

            var sql = @"
create table if not exists Country(
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    capital VARCHAR(255) NOT NULL,
    population INT NOT NULL
);";
            var command = new MySqlCommand(sql, connection);
            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}
