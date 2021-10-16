#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["./services/OnoStore.Catalog.API/OnoStore.Catalog.API.csproj", "src/services/OnoStore.Catalog.API/"]
COPY ["./building blocks/OnoStore.Core/OnoStore.Core.csproj", "building blocks/OnoStore.Core/"]
COPY ["./building blocks/OnoStore.WebAPI.Core/OnoStore.WebAPI.Core.csproj", "building blocks/OnoStore.WebAPI.Core/"]
COPY ["./building blocks/OnoStore.MessageBus/OnoStore.MessageBus.csproj", "building blocks/OnoStore.MessageBus/"]

RUN dotnet restore "src/services/OnoStore.Catalog.API/OnoStore.Catalog.API.csproj"
COPY . .
WORKDIR "services/OnoStore.Catalog.API"
RUN dotnet build "OnoStore.Catalog.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OnoStore.Catalog.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnoStore.Catalog.API.dll"]