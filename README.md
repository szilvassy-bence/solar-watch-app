# Solar-watch app

## About

This is a project which has the sole purpose of practice. It is a fullstack web application. I used the following stack to build it:
- backend: ASP NET web api, dotnet version: 8
- frontend: React with js, and MaterialUI
- database: MSSQL

## Structure
- The Program class is registering and configuring the application
- Loads environment variables in run without docker
- The Controllers wait for the requests and sends further to the Repositories
- The Repositories use further Services to transform JSON strings into Models
- The Repositories send back data to Controllers
- All the **_sensitive data are hidden_** inside .env file. The sample.env file has the same data as .env should has, for convenience.

## Run the app

### Run with Docker
1. Clone the repo
```
git clone https://github.com/szilvassy-bence/solar-watch-app.git
```
2. In the root directory rename \*sample.env\* to \*.env\*
> [!NOTE]
> You may use your own api key, and passwords, or just leave it as it is for convenience.
3. Run command:
```
docker-compose up
```
4. Go to http://localhost:8082/

### Without docker
1. Clone the repo
```
git clone https://github.com/szilvassy-bence/solar-watch-app.git
```
2. In the root directory rename \*sample.env\* to \*.env\*
> [!NOTE]
> You may use your own api key, and passwords, or just leave it as it is for convenience.
3. Start `mssql` in docker as usual.
```
docker run --name solar-watch-app-db-only -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Solar-Watch-2024" -p 1433:1433 -d mcr.microsoft.com/mssql/server
```
> [!NOTE]
> It is not recommended to change the MSSQL docker command. If you change the SA_PASSWORD in the MSSQL, then you must also change it in .env file.
4. Start dotnet backend: run \*backend project\*.
5. Start react frontend:
```
cd frontend
npm install
npm run dev
```


