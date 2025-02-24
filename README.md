# Bifrons - a BX lens library
A bidirectional lens library in C# .NET 8. This library is described in the paper "Bidirectionalization For The Common People" (BFTCP) and is used to introduce BX to a wider software development audience.

## Paper examples
The BFTCP paper provides code examples, which can be found here as individual unit tests for readers to freely try out for themselves:
[PaperExamples.cs](https://github.com/JurajDoncevic/Bifrons/blob/c30d0acd7137c94e4cf7592bf29e315ebdd0be0c/Bifrons.Experiments/PaperExamples.cs)

The tests for the paper examples can be run through the command line as follows: 
- position yourself into the `Experiments` project:\
  `cd ./Bifrons.Experiments`
- run just the tests from the paper examples:\
  `dotnet test --filter "FullyQualifiedName~Bifrons.Experiments.PaperExamples"`

The tests can also be run within an IDE of your choice that supports C# .NET.

## Database synchronization case study experiment
The BFTCP paper presents a case study experiment for synchronizing the data between two technologically and structurally heterogeneous databases with lenses. The experiment is provided as a unit test. The databases for financial and academic management are placed within two Docker containers, so running the tests doesn't depend on any local installations except for Docker itself.

The class `RelationalDataSyncExperiments` contains multiple unit test methods, each incrementing in functional complexity. The class method `Synchronize_StudentTableData_WhenLeftAndRightDataIsAdded` contains the case study experiment as is presented in the BFTCP paper.

The tests themselves take care of creating and running the docker containers, with the option to also pull and build the images for the databases' containers. This is achieved through `Testcontainers`.
Despite the ability to build the images on each experiment run, it is highly recommended to build the database container images beforehand to save time:

Build the database container images to keep them and avoid rebuilds on each test run.
The default settings for the experiment and the databases setup are found in the `appsettings.json` file. By default, this file initiates Docker image builds on each run - so it is advised to set the `"BuildImage"` properties to `false` and build the once manually.  

A local `appsettings.Local.json` configuration file can be created and it will be used as the locally default settings file. This is useful for playing around with the experiment setting.

**Prepare and run the BFTCP case study experiment:**
1) position into `./Bifrons.Experiments`*

2) build Docker image for AcademicManagement on postgres

`docker build -t bifrons-test-academic:latest -f Dockerfile.postgres --target academic_management_db .`

3) build Docker image for FinancialManagement on mysql

`docker build -t bifrons-test-financial:latest -f Dockerfile.mysql --target financial_management_db .`
> Change the targeted docker file according to the DB you wish to use - no need if you are OK with the default experiment setting. Don't forget to specify the changes in the `appsetting.*.json` file!

4) set `"BuildImage": false,` in the `appsettings.Local.json` or `appsettings.json`, as this avoids building Docker images on EVERY test run.

5) run the experiment test:
`dotnet test --filter "FullyQualifiedName~Bifrons.Experiments.RelationalDataSyncExperiments.Synchronize_StudentTableData_WhenLeftAndRightDataIsAdded"`

The test asserts a correct synchronization by counting the rows, which can be further manually verified by connecting to the databses with an SQL client application like [DBeaver](https://dbeaver.io/) and checking the `Students` tables.



