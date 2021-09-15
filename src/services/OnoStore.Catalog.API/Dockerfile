#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/services/OnoStore.Catalog.API/OnoStore.Catalog.API.csproj", "src/services/OnoStore.Catalog.API/"]
RUN dotnet restore "src/services/OnoStore.Catalog.API/OnoStore.Catalog.API.csproj"
COPY . .
WORKDIR "/src/src/services/OnoStore.Catalog.API"
RUN dotnet build "OnoStore.Catalog.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OnoStore.Catalog.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnoStore.Catalog.API.dll"]