
# Start Database up
1. open docker desktop
2. run docker run --name BookFace -e POSTGRES_PASSWORD=facebook -d -p 5432:5432 postgres
3. dotnet ef database update

To run in development set the ASPNETCORE_ENVIRONMENT environment variable to Development or set it for the duration of the program running like so:
```bash
env ASPNETCORE_ENVIRONMENT=Development dotnet run
```

dotnet run -e ASPNETCORE_ENVIRONMENT=development
