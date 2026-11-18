FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY S2VX.Web.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM caddy:2-alpine
COPY Caddyfile /etc/caddy/Caddyfile
COPY --from=build /app/publish/wwwroot /srv
