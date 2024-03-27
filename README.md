# PocketUSOS API

## Requirements

- **[Docker](https://docker.com/)** and **[docker-compose](https://docs.docker.com/compose/install/)**. They both can be installed by one tool - **[Docker Desktop](https://www.docker.com/products/docker-desktop/)**
- **[.Net CLI](https://learn.microsoft.com/en-us/dotnet/core/tools/)** and **[EF Tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)**

## Local setup

1. Clone Git repository:
    ```bash
    git clone git@github.com:pocket-usos/pocket-usos.git
    ```
2. Start docker containers:
   ```bash
   make up
   ```
3. Run database migrations:
    ```bash
    make db-update
    ```
4. Seed database:
   ```bash
   # TODO: seed database with institutions
   ```
5. Setup secrets:
   ```bash
    dotnet user-secrets init
   ```
   ```bash
    dotnet user-secrets set "Usos:Institutions:**InstitutionId**:ConsumerKey" "**ConsumerKey**" --project src/API
   ```
   ```bash
    dotnet user-secrets set "Usos:Institutions:**InstitutionId**:ConsumerSecret" "**ConsumerSecret**" --project src/API
   ```
