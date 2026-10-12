# Recruit Selector

![Tests](https://github.com/LukeNSaunders/recruit-selector/actions/workflows/tests.yml/badge.svg)

Works out which recruit has to come back next year. C# (.NET 10) API with a Vue 3 + TypeScript frontend.

**Live app:** https://recruit-selector-gsakcjg2eaaebuch.australiaeast-01.azurewebsites.net (login details sent separately)

**Note:** I've read "skipping k" literally: the sergeant skips k recruits and stops on the next one. So 5 recruits with k = 1 selects 2, 4, 1, 5 and recruit 3 comes back.

## Run with Docker

Needs Docker Desktop.

```
cp .env.example .env
docker compose up --build
```

Open http://localhost:8080 and log in with the username and password in `.env` (`demo` / `demo-password` by default).

## Run locally

Needs the .NET 10 SDK and Node.js 22+.

**Server:**

```
cd server
dotnet user-secrets set "Jwt:Key" "$(openssl rand -base64 32)"
dotnet user-secrets set "Auth:Username" "sergeant"
dotnet user-secrets set "Auth:Password" "next-year"
dotnet run
```

**Client** (in a second terminal):

```
cd client
npm install
npm run dev
```

Open http://localhost:5173.

## Tests

```
# C# unit and integration tests
cd server.Tests
dotnet test

# Lint and end-to-end tests
cd client
npm run lint
npx playwright install chromium
npm run test:e2e
```

Tests run on every push with GitHub Actions, and `main` is deployed to Azure when they pass.
