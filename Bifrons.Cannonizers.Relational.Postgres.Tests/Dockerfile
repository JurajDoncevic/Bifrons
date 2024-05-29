# Dockerfile
FROM postgis/postgis:latest

# Copy the database initialization script into the docker-entrypoint-initdb.d directory
COPY ./database_init.sql /docker-entrypoint-initdb.d/


# Set environment variables
ENV POSTGRES_PASSWORD=mysecretpassword
ENV POSTGRES_DB=poepleandroles
ENV POSTGRES_USER=postgres

# Expose the PostgreSQL port
EXPOSE 5432