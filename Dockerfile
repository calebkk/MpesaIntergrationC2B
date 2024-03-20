# Use the official Microsoft ASP.NET Core image as the base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Copy the published output of the ASP.NET application to the /app directory in the container
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish "MpesaIntergrationC2B.csproj" -c Release -o /app

# Set the entry point for the container
FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "MpesaIntergrationC2B.dll"]

