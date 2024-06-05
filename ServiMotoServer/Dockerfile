# Use the .NET SDK image to build the application and run migrations
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ServiMotoServer/ServiMotoServer.csproj", "ServiMotoServer/"]
RUN dotnet restore "./ServiMotoServer/ServiMotoServer.csproj"
COPY . .
WORKDIR "/src/ServiMotoServer"
RUN dotnet build "./ServiMotoServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Install dotnet-ef tool
RUN dotnet tool install --global dotnet-ef

# Make sure the dotnet tools are available in the PATH
ENV PATH="$PATH:/root/.dotnet/tools"

# Ensure the data directory exists and has proper permissions
RUN mkdir -p /app/data
RUN chmod -R 777 /app/data

# Set environment variable for connection string
ENV ConnectionStrings__DefaultConnection="Data Source=/app/data/servimoto.db"

# Run migrations
RUN dotnet publish "./ServiMotoServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
RUN dotnet ef database update --project ./ServiMotoServer.csproj

# Use the .NET runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Install sqlite3 in the final image
RUN apt-get update && apt-get install -y sqlite3

# Copy the build artifacts and the migrated database to the runtime image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY --from=build /app/data/servimoto.db /app/data/servimoto.db
VOLUME ["/app/data"]
ENTRYPOINT ["dotnet", "ServiMotoServer.dll"]
