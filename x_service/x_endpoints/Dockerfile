FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app
EXPOSE 8080

# copy .csproj and restore as destinct layers
COPY "Duelist-Pro.sln" "Duelist-Pro.sln"
COPY "API/API.csproj" "API/API.csproj"
COPY "Application/Application.csproj" "Application/Application.csproj"
COPY "Persistence/Persistence.csproj" "Persistence/Persistence.csproj"
COPY "Domain/Domain.csproj" "Domain/Domain.csproj"
COPY "Infrastructure/Infrastructure.csproj" "Infrastructure/Infrastructure.csproj"

RUN dotnet restore "Duelist-Pro.sln"

# copy everything else build the
COPY . .
WORKDIR /app
RUN dotnet publish -c Release -o out

# build a runtime image
FROM mcr.Microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "API.dll" ]