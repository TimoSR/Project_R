# Build image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy the solution file and restore dependencies
COPY "x_service.sln" "x_service.sln"
COPY "x_endpoints/" "x_endpoints/"
COPY "x_BL_tests/" "x_BL_tests/"
COPY "x_lib/" "x_lib/"
RUN dotnet restore

# copy everything else and build
COPY . .
RUN dotnet publish "x_endpoints/x_endpoints.csproj" -c Release -o out

# Runtime image
FROM mcr.microsoft.com/dotnet/sdk:7.0
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "x_endpoints.dll" ]