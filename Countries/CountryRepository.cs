using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Countries
{
    public class CountryRepository : IDisposable
    {
        public CountryRepository(IConfiguration configuration)
        {
            connection = new MySqlConnection(configuration.GetConnectionString("Db"));
            connection.Open();
        }

        public IEnumerable<Country> GetAll()
        {
            var command = connection.CreateCommand();
            command.CommandText = "select * from Country";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return new Country
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Name = Convert.ToString(reader["name"]),
                    Capital = Convert.ToString(reader["capital"]),
                    Population = Convert.ToInt32(reader["population"])
                };
            }
        }

        public int AddCountry(Country country)
        {
            var command = connection.CreateCommand();
            command.CommandText = "insert into Country(name, capital, population) values(@name, @capital, @population)";
            command.Parameters.AddWithValue("@name", country.Name);
            command.Parameters.AddWithValue("@capital", country.Capital);
            command.Parameters.AddWithValue("@population", country.Population);
            command.ExecuteNonQuery();

            return (int)command.LastInsertedId;
        }

        public void Delete(int countryId)
        {
            var command = connection.CreateCommand();
            command.CommandText = "delete from Country where id = @id";
            command.Parameters.AddWithValue("@id", countryId);
            command.ExecuteNonQuery();
        }

        public void Update(int countryId, Country country)
        {
            var command = connection.CreateCommand();
            command.CommandText = "update Country set name = @name, capital = @capital, population = @population where id = @id";
            command.Parameters.AddWithValue("@id", countryId);
            command.Parameters.AddWithValue("@name", country.Name);
            command.Parameters.AddWithValue("@capital", country.Capital);
            command.Parameters.AddWithValue("@population", country.Population);
            command.ExecuteNonQuery();
        }

        public void Dispose()
        {
            connection.Close();
        }

        private readonly MySqlConnection connection;
    }
}
