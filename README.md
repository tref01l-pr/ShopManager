# ShopManager
# ASP.NET Framework + SQL Server

## 🚀 Getting Started
### 1. Prerequisites
Ensure you have the following installed:
- Docker Descktop
- SQL Server 2022 Developer Edition or Express
- .net 9.0 sdk

### 2. Clone the Repository
```
git clone https://github.com/tref01l-pr/ShopManager.git
```

### 3. Create .env file
It should look like this:
```
DATABASE_URL=
JWT_SECRET=
```

### 4. Configure SQL Server
Go to your .env file and there enter your DATABASE_URL=

### 5. Configure your JWT_SECRET
Go to your .env file and there enter your JWT_SECRET= your secret must not contain spaces or other specific characters. Also, your secret must not be too long or too short. For example string with 72 chars

### 6. Update migrations and do data seeding
```
dotnet ef database update --project .\ShopManager.DataAccess.SqlServer\ --startup-project .\ShopManager.API\
cd ./ShopManager.API
dotnet run --seeddata
```
After running all the commands, you will add all the migrations to your database and do data seeding.
You can run your application and everything will work.

### 7. Using docker to run application
To run it locally use this commands:
```
docker build -t shop-manager .
docker container ls
docker run -p 8080:8080 shop-manager
```