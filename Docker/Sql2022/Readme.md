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
