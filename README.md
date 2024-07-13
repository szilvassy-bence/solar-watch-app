# Solar-watch app

## About

This is a project which has the sole purpose of practice. It is a fullstack web application. I used the following stack to build it:
    - backend: ASP NET web api, dotnet version: 8
    - frontend: React with js, and MaterialUI
    - database: MSSQL

## Structure
    - The Program class is registering and configuring the application
    - The Controllers wait for the requests and sends further to the Repositories
    - The Repositories use further Services to transform JSON strings into Models
    - The Repositories send back data to Controllers

## Run the app

### Without docker
    1. Start `mssql` in docker as usual.
        1.1. docker run --name solar-watch-app-db-only -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Solar-Watch-2024" -p 1433:1433 -d mcr.microsoft.com/mssql/server
    2. Start dotnet backend as usual.
    3. Start react frontend as usual.
