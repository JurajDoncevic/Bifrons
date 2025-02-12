# Bifrons
A bidirectional lens library in C#. This library is described in the paper "Bidirectionalization For The Common People".


## Database synchronization case study experiment
Build the database container  images to keep them and avoid rebuilds on each test run.
1) position into `/Bifrons.Experiments`*

2) build for AcademicManagement on postgres

`docker build -t bifrons-test-academic:latest -f Dockerfile.postgres --target academic_management_db .`

3) build for FinancialManagement on mysql

`docker build -t bifrons-test-financial:latest -f Dockerfile.mysql --target financial_management_db .`
> Change the targeted docker file according to the DB you wish to use. Don't forget to specify the changes in the `appsetting.*.json` file!

4) set `"BuildImage": false,` in appsettings.*.json
