# docker build --build-arg=DB_INIT_FILE_PATH=./academic_management.sql --build-arg=DB_NAME=AcademicManagement -t academic_management_db .
# Dockerfile
FROM postgres:latest
# Set the local db initialization script to /docker-entrypoint-initdb.d/
ARG DB_INIT_FILE_PATH=./academic_management.sql
ARG DB_NAME=AcademicManagement
# Copy the database initialization script into the docker-entrypoint-initdb.d directory
COPY DB_INIT_FILE_PATH /docker-entrypoint-initdb.d/database_init.sql

# Change the permissions of the database initialization script
RUN chmod 777 /docker-entrypoint-initdb.d/database_init.sql

# Set environment variables
ENV POSTGRES_PASSWORD=mysecretpassword
ENV POSTGRES_DB=DB_NAME
ENV POSTGRES_USER=postgres

# Expose the PostgreSQL port
EXPOSE 5432

