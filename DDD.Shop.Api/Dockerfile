FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DDD.Shop.Api/DDD.Shop.Api.csproj", "DDD.Shop.Api/"]
RUN dotnet restore "DDD.Shop.Api/DDD.Shop.Api.csproj"
COPY . .
WORKDIR "/src/DDD.Shop.Api"
RUN dotnet build "DDD.Shop.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DDD.Shop.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DDD.Shop.Api.dll"]
