FROM mcr.microsoft.com/mssql/server:2022-latest

ENV ACCEPT_EULA=Y
ENV MSSQL_SA_PASSWORD=Sol3admin
ENV MSSQL_PID=Developer
ENV MSSQL_TCP_PORT=1433

# set permissions on directories
# RUN chown -R mssql:mssql /var/opt/mssql
 
VOLUME ["/var/opt/mssql"]

COPY *.bak /var/opt/mssql

# switching to the mssql user
USER mssql

# CMD /opt/mssql/bin/sqlservr
# RUN /var/opt/mssql/bin/sqlservr 
# --accept-eula & ) | grep -q "Service Broker manager has started" 


## docker build .

# docker run
#  --env 'ACCEPT_EULA=Y'
#  --env 'MSSQL_SA_PASSWORD=Sol3admin'
#  --name 'PortalMsSql'
#  --publish 1433:1433
#  --volume PortalMsSqlData:/var/opt/mssql
#  --detach mcr.microsoft.com/mssql/server:2022-latest

# docker run --name Portal_SQL --publish 1433:1433 -detach portal/sql