
# Start Database up
1. Optional: Open docker desktop
2. Run the following to start the database locally using docker:
```bash
docker run --name BookFace -e POSTGRES_PASSWORD=facebook -d -p 5432:5432 postgres
```
Or if you've already made the container before:
```bash
docker start BookFace
```

3. Update the database tables:
```bash
dotnet ef database update
```

4. To run in development set the ASPNETCORE_ENVIRONMENT environment variable to Development or set it for the duration of the program running like so:
```bash
env ASPNETCORE_ENVIRONMENT=Development dotnet run --project src
```

dotnet run -e ASPNETCORE_ENVIRONMENT=development
