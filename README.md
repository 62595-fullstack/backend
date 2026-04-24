
# Start Database up

1. User-secrets
```bash
dotnet user-secrets set host localhost --project src
dotnet user-secrets set username postgres --project src
dotnet user-secrets set password facebook --project src
dotnet user-secrets set database BookFace --project src
dotnet user-secrets set port 5432 --project src
dotnet user-secrets set programPort 5000 --project src

dotnet user-secrets init --project tests
dotnet user-secrets set host localhost --project tests
dotnet user-secrets set port 5000 --project tests


test server 
dotnet user-secrets set host localhost --project src --id "test"
dotnet user-secrets set username postgres --project src --id "test"
dotnet user-secrets set password facebook --project src --id "test"
dotnet user-secrets set database BookFace --project src --id "test"
dotnet user-secrets set port 5432 --project src --id "test"
dotnet user-secrets set programPort 5000 --project src --id "test"

dotnet user-secrets init --project tests --id "test"
dotnet user-secrets set host localhost --project tests --id "test"
dotnet user-secrets set port 5000 --project tests --id "test"


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


Update user secrets
dotnet user-secrets set host localhost --project src
dotnet user-secrets set username postgres --project src
dotnet user-secrets set password facebook --project src
dotnet user-secrets set database BookFace --project src
dotnet user-secrets set port 5432 --project src
dotnet user-secrets set programPort 5000 --project src

dotnet user-secrets set testHost localhost --project tests
dotnet user-secrets set testPort 5000 --project tests