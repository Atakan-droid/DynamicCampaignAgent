# Use the official .NET 8 SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and restore as distinct layers
COPY DynamicCampaignAgent.sln ./
COPY WebApi/WebApi.csproj WebApi/
COPY Agents/Agents.csproj Agents/
COPY Data/Data.csproj Data/
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish WebApi/WebApi.csproj -c Release -o /app/publish /p:UseAppHost=false \
    && cp Data/MockData.json /app/publish/MockData.json

# Use the official ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose port 80 and 443
EXPOSE 80
EXPOSE 443

# Set environment variables (optional)
ENV ASPNETCORE_URLS="http://+:8080"

# Run the application
ENTRYPOINT ["dotnet", "WebApi.dll"] 