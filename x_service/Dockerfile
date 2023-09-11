# Build image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY "x_service_no_tests.sln" .
COPY "x_API/x_API.csproj" "x_API/"
COPY "x_Lib/x_Lib.csproj" "x_Lib/"
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish "x_API/x_API.csproj" -c Release -o out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "x_API.dll" ]