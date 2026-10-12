# ---------- 1. Build the Vue app ----------
FROM node:22-alpine AS client-build
WORKDIR /client
COPY client/package.json client/package-lock.json ./
RUN npm ci
COPY client/ ./
RUN npm run build

# ---------- 2. Build the C# server ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS server-build
WORKDIR /src
COPY server/Server.csproj server/
RUN dotnet restore server/Server.csproj
COPY server/ server/
RUN dotnet publish server/Server.csproj -c Release -o /app/publish

# ---------- 3. The final image that runs the app ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# The C# server...
COPY --from=server-build /app/publish ./
# ...plus the built Vue app, which the server hands out from its wwwroot folder
COPY --from=client-build /client/dist ./wwwroot

# .NET apps in containers listen on port 8080 by default
EXPOSE 8080
ENTRYPOINT ["dotnet", "Server.dll"]