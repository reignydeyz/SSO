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
- [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle)

### Prerequisites

- [Visual Studio](https://www.visualstudio.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-2022)
- [SQL Server Management Studio](https://msdn.microsoft.com/en-us/library/mt238290.aspx)
- [Node.js](https://nodejs.org)
- [.Net Core SDK](https://dotnet.microsoft.com/download)

### Debugging

- Make sure the **Startup project** is `SSO`.
- From `SSO`, open `appsettings.json` and change the **connection string** accordingly...
  ```json
  {
  "ConnectionStrings": {
    "DefaultConnection": "[CONNECTION_STRING]"
  }
  ```

- Database that includes the tables and sample data will be automatically generated.
- Run by pressing **F5**. Login page should be launched.
  - For credentials, see sample data in `SSO.Infrastructure\Migrations\xxxxxxxxxxxxxx_Seed.cs`
- For **Swagger**, go to `{URL}/swagger`

## Documentations
// TBD


## People to blame

The following personnel is/are responsible for managing this project.

- [actchua@periapsys.com](mailto:actchua@periapsys.com)