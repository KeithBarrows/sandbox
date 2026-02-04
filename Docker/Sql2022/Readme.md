# Setup SQL2022 Developer Edition in a Docker Container

`docker run --name MSSQL2022 --hostname MSSQL2022 -d --restart unless-stopped -e ACCEPT_EULA=Y -e MSSQL_SA_PASSWORD="Sol3admin" -v C:\data\mssql\2022\portal:/var/opt/mssql -v C:\data\mssql\2022\portal\data:/var/opt/mssql/data -v C:\data\mssql\2022\portal\logs:/var/opt/mssql/logs -v C:\data\mssql\2022\portal\backup:/var/opt/mssql/backup -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest`

`docker run --name MSSQL2022 --hostname MSSQL2022 -d --restart unless-stopped -e ACCEPT_EULA=Y -e MSSQL_SA_PASSWORD="Sol3admin" -v C:\data\mssql\2022\portal:/var/opt/mssql -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest`

`docker run --env 'ACCEPT_EULA=Y' --env 'MSSQL_SA_PASSWORD=Sol3admin' --name 'PortalMsSql' --publish 1433:1433 --volume PortalMsSqlData:/var/opt/mssql --detach mcr.microsoft.com/mssql/server:2022-latest`

- docker exec PortalMsSql bash -c 'ps -aux'
- docker exec PortalMsSql bash -c 'id mssql' 
- docker cp ./portal_full_116_final_20241014.bak PortalMsSqlData:/var/opt/mssql/data
- docker exec PortalMsSql bash -c 'ls -lan /var/opt/mssql/data/*'
- docker exec -u 0 PortalMsSql bash -c 'chown 10001:0 /var/opt/mssql/data/*'
- docker exec -u 0 PortalMsSql bash -c 'chmod 660 /var/opt/mssql/data/*'

----

- docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<YourStrong@Passw0rd>" -p 1433:1433 --name sql1 --hostname sql1 --restart unless-stopped  -v sql1data:/var/opt/mssql -d mcr.microsoft.com/mssql/server:2022-latest
- docker exec -it sql1 mkdir /var/opt/mssql/backup
- docker cp wwi.bak sql1:/var/opt/mssql/backup
- docker exec -it sql1 /opt/mssql-tools/bin/sqlcmd -S localhost \
   -U SA -P '<YourNewStrong!Passw0rd>' \
   -Q 'RESTORE FILELISTONLY FROM DISK = "/var/opt/mssql/backup/wwi.bak"' \
   | tr -s ' ' | cut -d ' ' -f 1-2
- [read this will help you](https://learn.microsoft.com/en-us/sql/linux/tutorial-restore-backup-in-sql-server-container?view=sql-server-ver16&tabs=cli) 
- [Github: mssql-customize](https://github.com/microsoft/mssql-docker/tree/master/linux/preview/examples/mssql-customize)
- [The cleanest way to use Docker for testing in .NET](https://www.youtube.com/watch?v=8IRNC7qZBmk)