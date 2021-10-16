#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
#COPY ["src/web/OnoStore.WebApp.MVC/OnoStore.WebApp.MVC.csproj", "src/web/OnoStore.WebApp.MVC/"]
# working with this project COPY ["OnoStore.WebApp.MVC.csproj", "src/web/OnoStore.WebApp.MVC/"]
# working with docker-compose
COPY ["./web/OnoStore.WebApp.MVC/OnoStore.WebApp.MVC.csproj", "web/OnoStore.WebApp.MVC/"]
COPY ["./building blocks/OnoStore.Core/OnoStore.Core.csproj", "building blocks/OnoStore.Core/"]
COPY ["./building blocks/OnoStore.WebAPI.Core/OnoStore.WebAPI.Core.csproj", "building blocks/OnoStore.WebAPI.Core/"]

#RUN dotnet restore "src/web/OnoStore.WebApp.MVC/OnoStore.WebApp.MVC.csproj"
RUN dotnet restore "web/OnoStore.WebApp.MVC/OnoStore.WebApp.MVC.csproj"
COPY . .
#WORKDIR "/src/src/web/OnoStore.WebApp.MVC"
WORKDIR "web/OnoStore.WebApp.MVC"
RUN dotnet build "OnoStore.WebApp.MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OnoStore.WebApp.MVC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnoStore.WebApp.MVC.dll"]

# To compile:
# docker build .

# Run from the root folder (3 steps above of this one)
# docker build ./src/web/OnoStore.WebApp.MVC   

# docker-compose -f file.yml up