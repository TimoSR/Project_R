FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app
EXPOSE 8080

# Copy the solution file and restore dependencies
COPY "x_service.sln" .

# Copy the projects and restore their dependencies
COPY "x_endpoints/" "x_endpoints/"
COPY "x_lib/" "x_lib/"
RUN dotnet restore

# copy everything else build the
COPY . .
RUN dotnet publish -c Release -o out

# build a runtime image
FROM mcr.microsoft.com/dotnet/sdk:7.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "x_endpoints.dll" ]