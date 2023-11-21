#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

ARG VERSION=6.0-alpine
FROM mcr.microsoft.com/dotnet/aspnet:$VERSION AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:$VERSION AS build
WORKDIR /src
COPY ["Movies/Movies.csproj", "Movies/"]
RUN dotnet restore "Movies/Movies.csproj"
COPY . .
WORKDIR "/src/Movies"
RUN dotnet build "Movies.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Movies.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Movies.dll"]