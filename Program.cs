// TODO run the database please use 
// docker run --name BookFace -e POSTGRES_PASSWORD=facebook -d -p 5432:5432 postgres
// dotnet ef database update

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Linq;

IConfigurationRoot config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var connString = $@"Host={config["host"]};
                 Username={config["username"]};
                 Password={config["password"]};
                 Database={config["database"]}";

await using var conn = new NpgsqlConnection(connString);
await conn.OpenAsync();

// await using (var cmd = new NpgsqlCommand("INSERT INTO data (some_field) VALUES (@p)", conn))
// {
//     cmd.Parameters.AddWithValue("p", "Hello world");
//     await cmd.ExecuteNonQueryAsync();
// }
//
// // Retrieve all rows
// await using (var cmd = new NpgsqlCommand("SELECT some_field FROM data", conn))
// await using (var reader = await cmd.ExecuteReaderAsync())
// {
// while (await reader.ReadAsync())
//     Console.WriteLine(reader.GetString(0));
// }
