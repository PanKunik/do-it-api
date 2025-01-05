FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /
COPY ./DoIt.sln .

WORKDIR src
COPY ./src/DoIt.Api/DoIt.Api.csproj ./DoIt.Api/

WORKDIR ../tests
COPY ./tests/DoIt.Api.TestUtils/DoIt.Api.TestUtils.csproj ./DoIt.Api.TestUtils/
COPY ./tests/DoIt.Api.Unit.Tests/DoIt.Api.Unit.Tests.csproj ./DoIt.Api.Unit.Tests/
COPY ./tests/DoIt.Api.Integration.Tests/DoIt.Api.Integration.Tests.csproj ./DoIt.Api.Integration.Tests/

WORKDIR /
RUN dotnet restore

COPY . .
RUN dotnet build --no-restore -c Release -o /app/build

RUN dotnet test --no-restore

FROM build AS publish
RUN dotnet publish --no-restore DoIt.sln -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 5000
ENV ASPNETCORE_FORWARDERHEADERS_ENABLED=true
ENV ASPNETCORE_HTTP_PORTS=5000
ENTRYPOINT [ "dotnet", "DoIt.Api.dll" ]