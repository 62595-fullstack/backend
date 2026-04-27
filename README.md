# Start Database up

1. Secrets
1.1 DotNET user-secrets
```bash
dotnet user-secrets set host localhost --project src
dotnet user-secrets set username postgres --project src
dotnet user-secrets set password facebook --project src
dotnet user-secrets set database BookFace --project src
dotnet user-secrets set port 5432 --project src
dotnet user-secrets set programPort 5000 --project src
```

1.2 Environment variables
Linux
```bash
export host=localhost port=5432 username=postgres password=facebook database=BookFace programport=5000
```
Windows
```bash
set host=localhost port=5432 username=postgres password=facebook database=BookFace programport=5000
```

2. Optional: Open docker desktop
3. Run the following to start the database locally using docker:
```bash
docker run --name BookFace -e POSTGRES_PASSWORD=facebook -d -p 5432:5432 postgres
```
Or if you've already made the container before:
```bash
docker start BookFace
```

4. Update the database tables:
```bash
dotnet ef database update
```

5. To run in development set the ASPNETCORE_ENVIRONMENT environment variable to Development or set it for the duration of the program running like so:
```bash
dotnet run -e ASPNETCORE_ENVIRONMENT=development --project src
```

