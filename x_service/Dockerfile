FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app
EXPOSE 8080

# copy .csproj and restore as destinct layers
COPY "x_service.sln" "x_service.sln"
COPY "x_endpoints/x_endpoints.csproj" "x_endpoints/x_endpoints.csproj"
COPY "x_lib/x_lib.csproj" "x_lib/x_lib.csproj"
COPY "x_BL_tests/x_BL_tests.csproj" "x_BL_tests/x_BL_tests.csproj"

RUN dotnet restore "x_service.sln"

# copy everything else build the
COPY . .
WORKDIR /app
RUN dotnet publish -c Release -o out

# build a runtime image
FROM mcr.microsoft.com/dotnet/sdk:7.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "x_endpoints.dll" ]