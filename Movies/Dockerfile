#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:8.0-windowsservercore-ltsc2022 AS base
USER app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0-windowsservercore-ltsc2022 AS build
WORKDIR /src
COPY ["Movies.csproj", "."]
RUN dotnet restore
COPY . . 
WORKDIR /src/
RUN dotnet build "./Movies.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c release -o /app/publish /p:UseAppHost=false

FROM build
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Movies.dll"]