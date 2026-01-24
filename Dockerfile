# Use the official .NET 10 SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files
COPY CampaignAgentSolution.sln ./
COPY WebApi/WebApi.csproj WebApi/
COPY Agents/Agents.csproj Agents/
COPY Data/Data.csproj Data/
COPY Services/Services.csproj Services/
COPY Core/Core.csproj Core/

# Restore dependencies
RUN dotnet restore CampaignAgentSolution.sln

# Copy the remaining source code and publish
COPY . .
RUN dotnet publish WebApi/WebApi.csproj -c Release -o /app/publish /p:UseAppHost=false

# Use the official ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose port 80
EXPOSE 80

# Configure runtime environment
ENV ASPNETCORE_URLS="http://+:8080"
ENV DOTNET_RUNNING_IN_CONTAINER=true

# Run the application
ENTRYPOINT ["dotnet", "WebApi.dll"]