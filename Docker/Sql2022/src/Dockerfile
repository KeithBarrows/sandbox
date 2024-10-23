##----------------------------------------------##
## Create Data Volume First                     ##
## docker volume create portal_bkportal_sql2022 ##
##----------------------------------------------##


### Start with operating system
FROM mcr.microsoft.com/mssql/server:2022-latest
RUN echo `Using the stock/latest Sql Server 2022 image from docker-hub`

RUN echo $(MSSQL_SA_PASSWORD)

ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=$(MSSQL_SA_PASSWORD)
ENV MSSQL_PID=Developer
ENV MSSQL_TCP_PORT=1433

### Install language runtime
### Install application dependencies
COPY *.bak /var/opt/mssql/backup

### Setup execution environment
### Run application


# WORKDIR /src
# COPY filldata.sql ./filldata.sql
# RUN (/opt/mssql/bin/sqlservr --accept-eula & ) | grep -q "Service Broker manager has started" 


## docker build .