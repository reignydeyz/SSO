# SSO

Single sign-on (SSO) is web solution that centralizes authentication process. It allows users to access multiple applications or services with a single set of login credentials. In a nutshell, here's how SSO works:

- **User Authentication**: The user logs in once to a central identity provider (IDP) or authentication server.

- **Token Generation**: Upon successful authentication, the IDP generates a security token (e.g., a token based on Security Assertion Markup Language - SAML or JSON Web Token - JWT).

- **Token Validation**: The user presents this token to other applications or services they want to access.

- **Access Granted**: The applications or services validate the token with the IDP. If the token is valid, the user is granted access without having to log in again.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Technology Used

- [.Net 8](https://www.microsoft.com/net/download/windows)
- [Vue 3](https://vuejs.org/guide/introduction.html)
- [EntityFramework Core](https://docs.microsoft.com/en-us/ef/core/)
- [ASP.Net Identity](https://www.asp.net/identity)
- [OData 8](https://learn.microsoft.com/en-us/odata/webapi-8/overview)
- [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle)
- [Bootstrap 5.3](https://getbootstrap.com)
- [Hangfire](https://www.hangfire.io/)

### Prerequisites

- [Visual Studio](https://www.visualstudio.com/)
- [VS Code](https://code.visualstudio.com) - optional but recommended
- ~~[SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-2022)~~
- ~~[SQL Server Management Studio](https://msdn.microsoft.com/en-us/library/mt238290.aspx)~~
- [Node.js](https://nodejs.org)
- [.Net Core SDK](https://dotnet.microsoft.com/download)
- Supports **MSSQL**, **MySQL** and **Postgres**

### Debugging

#### Back-end
- Make sure the **Startup project** is `SSO`.
- From `SSO`, open `appsettings.json` and change the **connection string** accordingly...
  ```json
  "ConnectionStrings": {
    "DefaultConnection": "[CONNECTION_STRING]"
  }
  ```
    - For Postgres, use *Host* as server.
      ```
      Host=myServerAddress;Port=myPortNumber;Database=myDataBase;Username=myUsername;Password=myPassword;
      ```
    
  LDAP settings ~ If present, will use LDAP modules instead of the default ones...
  ```json
  "LDAPSettings": 
  {
    "Server": "example.com", // provide only the domain
    "Port": 389, // or the appropriate port for your LDAP server
    "Username": "your-ldap-username",
    "Password": "your-ldap-password",
    "SearchBase": "ou=users,dc=example,dc=com",
    "SearchFilter": "(sAMAccountName={0})", // or an appropriate LDAP filter for your system
    "UseSSL": false // set to true if using LDAPS (LDAP over SSL)
  }
  ```

- Update database (EF)...
  - PMC
    - From **Package Manager Console**, choose the Default project...
      - `SSO.Infrastructure` for MS SQL
      - `SSO.Infrastructure.Db.MySql` for MySQL
      - `SSO.Infrastructure.Db.Postgres` for Postgres
    - Generate the tables and data, run... (EF)
      ```
      update-database
      ```
  - Powershell
    - Locate the corresponding folder
      - `..\SSO.Infrastructure` for MS SQL
      - `..\SSO.Infrastructure.Db.MySql` for MySQL
      - `..\SSO.Infrastructure.Db.Postgres` for Postgres
    - Generate the tables and data, run... (EF)
      ```
      dotnet ef database update --startup-project ../SSO
      ```
- Run by pressing **F5**. Login page should be launched.
  - username: `admin@example.com`
  - password: `Password123#`
  
    (Username modifications are required due to the removal of special characters and the disallowance of email formats. Please make the necessary adjustments.)
- For **Swagger**, go to `{URL}/swagger`

#### Front-end
`SSO` project also contains the front-end that uses **Vue 3**. It is located in `SSO\ClientApp`.

**Note:** `npm` commands will be automatically executed upon launching the solution, ensuring both back-end and front-end are running on a single host.

However, it can be challenging to debug both front-end and back-end, so it is recommended to create a front-end (debug) instance.
- Open `SSO\ClientApp` in **VS Code**
- Ensure dependencies are installed, run...
  ```bash
  npm install --force
  ```
- Check `package.json` for port configuration and environment. Run either...
  ```bash
  npm run serve-dev
  ```
  or
  ```bash
  npm run serve-dev1
  ```

## Deployment

This tutorial shows how to host SSO on an IIS server.

### Prerequisites

The following must be installed on the server for SSO to work...
- [ASP.NET Core 8.0 Runtime (v8.0.3) - Windows Hosting Bundle Installer](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-aspnetcore-8.0.3-windows-hosting-bundle-installer)
- [Node.js](https://nodejs.org)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-2022)

### Publishing

Create the artifact to be deployed...
- From Visual Studio, right-click `SSO` project then click **Publish**
- Choose **Folder** as the Target. It should now create a Publish Profile.
- Click **Publish**. VS should compile and create the necessary binaries/libraries.
- Click **Target Location** to open the root folder of the generated binaries/libraries.
- Open `appsettings.json` and apply the necessary settings.
  - (Database will be automatically generated based on the `connectionString`)

### IIS 

Please visit [this link](https://learn.microsoft.com/en-us/aspnet/core/tutorials/publish-to-iis?view=aspnetcore-8.0&tabs=visual-studio#install-the-net-core-hosting-bundle)


## Documentations

Please visit [wiki page](https://github.com/reignydeyz/SSO/wiki)

## Misc
[![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=reignydeyz_SSO)](https://sonarcloud.io/summary/new_code?id=reignydeyz_SSO)