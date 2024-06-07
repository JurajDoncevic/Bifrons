# Dockerfile
FROM mysql:latest

# Copy the database initialization script into the docker-entrypoint-initdb.d directory
COPY ./database_init.sql /docker-entrypoint-initdb.d/

# Set environment variables
ENV MYSQL_ROOT_PASSWORD=mysecretpassword
ENV MYSQL_DATABASE=peopleandroles
ENV MYSQL_USER=admin
ENV MYSQL_PASSWORD=mysecretpassword

# Expose the MySQL port
EXPOSE 3306