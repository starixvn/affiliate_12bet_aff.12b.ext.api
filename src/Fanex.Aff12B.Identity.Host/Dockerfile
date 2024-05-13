FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder

WORKDIR "/src"

RUN dotnet nuget add source https://nuget.nexdev.net/api/v2

COPY ["src/Fanex.Aff12B.Common.Core/Fanex.Aff12B.Common.Core.csproj", "./Fanex.Aff12B.Common.Core/"]
RUN dotnet restore "Fanex.Aff12B.Common.Core/Fanex.Aff12B.Common.Core.csproj" -v n

COPY ["src/Fanex.Aff12B.Identity.Core/Fanex.Aff12B.Identity.Core.csproj", "./Fanex.Aff12B.Identity.Core/"]
RUN dotnet restore "Fanex.Aff12B.Identity.Core/Fanex.Aff12B.Identity.Core.csproj" -v n

COPY ["src/Fanex.Aff12B.Identity.Host/Fanex.Aff12B.Identity.Host.csproj", "./Fanex.Aff12B.Identity.Host/"]
RUN dotnet restore "Fanex.Aff12B.Identity.Host/Fanex.Aff12B.Identity.Host.csproj" -v n

COPY "/src" .

WORKDIR "/src/Fanex.Aff12B.Identity.Host"
RUN dotnet build "Fanex.Aff12B.Identity.Host.csproj" -c Release -o /app/build

RUN dotnet publish "Fanex.Aff12B.Identity.Host.csproj" -c Release -o /app/publish

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine

ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

# Don’t run as root
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app && chown -R dotnetuser /tmp
USER dotnetuser
#

WORKDIR "/app"

COPY --from=builder /app/publish .
ENV TZ="America/La_Paz"

ENTRYPOINT ["dotnet", "Fanex.Aff12B.Identity.Host.dll"]
