Here's a step-by-step guide:
1. Pull the PostgreSQL Docker image:

```docker
docker pull postgres
```

2. Run a PostgreSQL container:

```docker
docker run --name <container_name> -e POSTGRES_PASSWORD=<your_password> -p 5432:5432 -d postgres
```
- --name <container_name>: Assigns a name to your container (e.g., my-postgres-db). 
- -e POSTGRES_PASSWORD=<your_password>: Sets the password for the default postgres user. Replace <your_password> with your desired password. 
- -p 5432:5432: Maps port 5432 on your host machine to port 5432 in the container, allowing connections to the PostgreSQL server. 
- -d postgres: Runs the container in detached mode (in the background). 

3. (Optional) Add a username and database:

```docker
docker run --name <container_name> -e POSTGRES_PASSWORD=<your_password> -e POSTGRES_USER=<your_username> -e 
POSTGRES_DB=<your_database> -p 5432:5432 -d postgres
```
- -e POSTGRES_USER=<your_username>: Sets the username for the database. 
- -e POSTGRES_DB=<your_database>: Sets the name of the initial database. 