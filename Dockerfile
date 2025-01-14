FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /Api
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /Api/publish

FROM base AS final
WORKDIR /Api
COPY --from=build /Api/publish .
ENTRYPOINT ["dotnet", "MinimalApi.dll"]
