
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://*80
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CurrencyAPI/CurrencyAPI.csproj", "CurrencyAPI/"]
COPY ["Currency.BLL/Currency.BLL.csproj", "Currency.BLL/"]
COPY ["Currency.DAL/Currency.DAL.csproj", "Currency.DAL/"]
RUN dotnet restore "./CurrencyAPI/CurrencyAPI.csproj"
COPY . .
WORKDIR "/src/CurrencyAPI"
RUN dotnet build "CurrencyAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CurrencyAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CurrencyAPI.dll"]