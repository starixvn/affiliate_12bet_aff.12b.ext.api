ARG CI_REGISTRY_IMAGE

FROM $CI_REGISTRY_IMAGE:builder AS deps

RUN mkdir -p /src/packages

FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder

ENV NUGET_PACKAGES=/src/packages

COPY --from=deps $NUGET_PACKAGES/. $NUGET_PACKAGES/

WORKDIR "/src"

RUN dotnet nuget add source https://nuget.nexdev.net/api/v2

COPY ["src/Fanex.Aff12B.Common.Core/Fanex.Aff12B.Common.Core.csproj", "./Fanex.Aff12B.Common.Core/"]
RUN dotnet restore -v n -r linux-x64 "./Fanex.Aff12B.Common.Core/Fanex.Aff12B.Common.Core.csproj"

COPY ["src/Fanex.Aff12B.Api.Core/Fanex.Aff12B.Api.Core.csproj", "./Fanex.Aff12B.Api.Core/"]
RUN dotnet restore -v n -r linux-x64 "./Fanex.Aff12B.Api.Core/Fanex.Aff12B.Api.Core.csproj"

COPY ["src/Fanex.Aff12B.Api.Host/Fanex.Aff12B.Api.Host.csproj", "./Fanex.Aff12B.Api.Host/"]
RUN dotnet restore -v n -r linux-x64 "./Fanex.Aff12B.Api.Host/Fanex.Aff12B.Api.Host.csproj"

COPY "/src" .

WORKDIR "/src/Fanex.Aff12B.Api.Host"

RUN dotnet publish -r linux-x64 --self-contained false -o /app/publish

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

ENTRYPOINT ["dotnet", "Fanex.Aff12B.Api.Host.dll"]
