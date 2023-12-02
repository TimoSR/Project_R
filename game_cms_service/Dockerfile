# Build image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy the solution file and necessary projects
COPY x_service.sln .
COPY x_endpoints/x_endpoints.csproj x_endpoints/
COPY x_BL_tests/x_BL_tests.csproj x_BL_tests/
COPY x_lib/x_lib.csproj x_lib/
RUN dotnet restore

# Copy the source code and publish the application
COPY x_endpoints/ x_endpoints/
COPY x_lib/ x_lib/
RUN dotnet publish x_endpoints/x_endpoints.csproj -c Release -o out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "x_endpoints.dll"]