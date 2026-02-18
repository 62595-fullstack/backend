
# Start Database up
1. open docker desktop
2. run docker run --name BookFace -e POSTGRES_PASSWORD=facebook -d -p 5432:5432 postgres
3. dotnet ef database update
