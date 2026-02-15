// TODO run the database please use 
// docker run --name BookFace -e POSTGRES_PASSWORD=facebook -d -p 5432:5432 postgres
// dotnet ef database update

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;


var connString = "Host=localhost;Username=facebook;Password=1234;Database=testdb";

await using var conn = new DatabaseContext(connString);
await conn.OpenAsync();

// // Insert some data
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
