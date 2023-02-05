#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["KitchenConnection.Notifier/KitchenConnection.Notifier.csproj", "KitchenConnection.Notifier/"]
COPY ["KitchenConnection.Models/KitchenConnection.Models.csproj", "KitchenConnection.Models/"]
RUN dotnet restore "KitchenConnection.Notifier/KitchenConnection.Notifier.csproj"
COPY . .
WORKDIR "/src/KitchenConnection.Notifier"
RUN dotnet build "KitchenConnection.Notifier.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KitchenConnection.Notifier.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KitchenConnection.Notifier.dll"]