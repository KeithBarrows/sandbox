# Setup Seq in a Docker Container

In a terminal of your choice (Windows Terminal, Powershell, VSCode, etc):

- Step 1 - run the following command to create a hashed password:  
`echo '<YOUR!P@SSW0RD>' | docker run --rm -i datalust/seq config hash`  
Copy the result

- Step 2 - create a data directory for the Seq ingestion files:  
`mkdir -p c:\data\seq`

- Step 3 - run the docker command  
`docker run --name seq -d --restart unless-stopped -e ACCEPT_EULA=Y -e SEQ_FIRSTRUN_ADMINPASSWORDHASH="<STEP 1>" -v c:\data\seq:/data -p 5341:80 datalust/seq`
