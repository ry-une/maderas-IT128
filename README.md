# IT128 Coursework

Academic programming exercises by **Ryan Joseph V. Maderas**, BS Information Technology.

Section: F0PM01

## Repository guide

| Location | Purpose |
| :--- | :--- |
| [index.html](index.html) | HTML page from the coursework |
| [MaderasLE2](MaderasLE2/) | C# solution and database project |
| [BlogDataLibrary](MaderasLE2/BlogDataLibrary/) | Data, database-access, and model components |
| [BlogTestUI](MaderasLE2/BlogTestUI/) | Console application for exercising blog operations |
| [MaderasBlogDB](MaderasLE2/MaderasBlogDB/) | Database project |

## Application context

The console project targets `net6.0` and references `BlogDataLibrary`. Its current entry point invokes the add-post flow. Additional methods cover authentication, registration, post listing, and post details; they are not all invoked by the entry point.

Configuration is read from `appsettings.json` in the working directory. The checked-in connection string targets SQL Server LocalDB, database `BlogDB`, using Windows-integrated authentication. Running the application requires the matching database schema and suitable local .NET and database tooling.

Build, database deployment, and execution have not been verified as part of this documentation update. `BlogTestUI` is a console exercise application, not an automated test suite.

## Learning-project limitations

This is coursework, not a production authentication service.

- The add-post flow does not check an unsuccessful authentication result before accessing the user's ID.
- Password input is read through the console without masking.
- Use only disposable local test data and credentials.
