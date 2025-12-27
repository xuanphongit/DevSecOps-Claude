# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["SecureShop.API/SecureShop.API.csproj", "SecureShop.API/"]
RUN dotnet restore "SecureShop.API/SecureShop.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/SecureShop.API"
RUN dotnet build "SecureShop.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "SecureShop.API.csproj" -c Release -o /app/publish \
    -p:PublishTrimmed=true

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

# Create non-root user
RUN addgroup -g 1001 appgroup && \
    adduser -u 1001 -G appgroup -s /bin/sh -D appuser

# Create logs directory
RUN mkdir -p /app/logs && \
    chown -R appuser:appgroup /app/logs

# Copy published app
COPY --from=publish /app/publish .

# Set ownership
RUN chown -R appuser:appgroup /app

# Switch to non-root user
USER appuser

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=10s --retries=3 \
  CMD wget --no-verbose --tries=1 --spider http://localhost:8080/api/health || exit 1

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "SecureShop.API.dll"]

