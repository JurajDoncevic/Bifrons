# Dockerfile
FROM mcr.microsoft.com/mssql/server:2019-latest

# Set environment variables
ENV ACCEPT_EULA=Y
ENV MSSQL_PID=Express
ENV SA_PASSWORD=ComplexPassword123!
ENV MSSQL_PASSWORD=mysecretpassword
ENV MSSQL_DB=PeopleAndRoles
ENV MSSQL_USER=admin

# Copy the database initialization script
COPY ./database_init.sql /database_init.sql

# Run the SQL Server process
# CMD /opt/mssql/bin/sqlservr

# Run the SQL Server process
# Create the database
# Create a new user with sysadmin privileges - workaround for the issue with the password not meeting the complexity requirements
# Run the database initialization script
ENTRYPOINT [ "/bin/bash", "-c", "(/opt/mssql/bin/sqlservr --accept-eula & ) | grep -q 'Service Broker manager has started' && sleep 30 && /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $SA_PASSWORD -Q 'CREATE DATABASE $MSSQL_DB' && /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $SA_PASSWORD -Q 'CREATE LOGIN $MSSQL_USER WITH PASSWORD = $MSSQL_PASSWORD, CHECK_POLICY = OFF; ALTER LOGIN $MSSQL_USER WITH PASSWORD = $MSSQL_PASSWORD; ALTER SERVER ROLE sysadmin ADD MEMBER $MSSQL_USER;' && /opt/mssql-tools/bin/sqlcmd -S localhost -U $MSSQL_USER -P $MSSQL_PASSWORD -i /database_init.sql" ]
# Expose the MSSQL port
EXPOSE 1433