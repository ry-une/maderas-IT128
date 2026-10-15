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


The console project builds with the .NET 10 SDK, with existing warnings about its unsupported .NET 6 target, dependency compatibility, and nullable values. Database deployment and live SQL execution remain unverified. `BlogTestUI` is a console exercise application.


## Learning-project limitations


This is coursework, not a production authentication service.


- The add-post flow rejects unsuccessful authentication before requesting post content or writing to the database.
- Password input is read through the console without masking.
- Use only disposable local test data and credentials.

## Manual verification checklist

This is a proposed local procedure, not a record of completed tests. Use a disposable database and synthetic users only.

1. Record the commit, operating system, installed .NET tooling, database tooling, and database schema used. Confirm the working directory contains the intended `appsettings.json`.
2. Build the solution and record the actual result. If setup or build fails, capture the error and mark dependent checks as blocked.
3. With a synthetic account and valid credentials, exercise the current add-post flow. Record the console outcome and verify whether exactly one expected post is stored for that account.
4. Repeat with invalid credentials. Confirm it reports invalid credentials, does not request a title/body, and creates no post. Do not record a pass without observing the behavior.
5. Record whether password characters are visible during input. Do not capture actual passwords in screenshots or notes.
6. Record any database-connectivity failure separately from an authentication or application failure.

For each check, record: date, commit, scenario, expected outcome, observed outcome, status (not run / blocked / pass / fail), and a sanitized evidence reference. Methods not invoked by the current entry point require a separately scoped test harness or code change before they can be exercised.

## Add-post regression checks

Run `dotnet run --project tests/BlogRegression` with a compatible .NET SDK and .NET 6 runtime. The dependency-free console harness invokes the actual add-post method with synthetic console input and a fake implementation of the existing database interface. It checks rejected logins produce no writes or content prompts, and accepted logins preserve the author, title, body, and one-write behavior. Failures return a nonzero exit code.

Both scenarios passed locally. The rejected-login scenario reproduced a null-reference exception before the fix. These checks do not validate SQL Server, stored procedures, credential storage, or deployed authentication.
