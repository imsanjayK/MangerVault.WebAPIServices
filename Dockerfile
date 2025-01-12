# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
ENV MONGO_PUBLIC_URL=mongodb+srv://imsanjayk95:P5fq3GCdRcUUfVxz@cluster0.enpdz.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0
ENV MONGO_DATABASE_NAME=vault
ENV SUBSCRIPTION_KEY=bb5f9256-1834-4a26-b65c-e48fcaccec32

EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MangerVault.WebAPIServices/MangerVault.WebAPIServices.csproj", "MangerVault.WebAPIServices/"]
RUN dotnet restore "./MangerVault.WebAPIServices/MangerVault.WebAPIServices.csproj"
COPY . .
WORKDIR "/src/MangerVault.WebAPIServices"
RUN dotnet build "./MangerVault.WebAPIServices.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MangerVault.WebAPIServices.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MangerVault.WebAPIServices.dll"]