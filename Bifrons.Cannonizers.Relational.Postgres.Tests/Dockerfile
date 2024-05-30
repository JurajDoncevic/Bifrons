# Dockerfile
FROM postgres:latest

# Copy the database initialization script into the docker-entrypoint-initdb.d directory
COPY ./database_init.sql /docker-entrypoint-initdb.d/

# Change the permissions of the database initialization script
RUN chmod 777 /docker-entrypoint-initdb.d/database_init.sql

# Set environment variables
ENV POSTGRES_PASSWORD=mysecretpassword
ENV POSTGRES_DB=poepleandroles
ENV POSTGRES_USER=postgres

# Expose the PostgreSQL port
EXPOSE 5432