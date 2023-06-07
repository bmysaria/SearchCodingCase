FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SimonVossSearch.Api/SimonVossSearch.Api.csproj", "SimonVossSearch.Api/"]
COPY ["SimonVossSearch.Core/SimonVossSearch.Core.csproj", "SimonVossSearch.Core/"]
COPY ["SimonVossSearch.Persistence/SimonVossSearch.Persistence.csproj", "SimonVossSearch.Persistence/"]
COPY ["SimonVossSearch.Domain/SimonVossSearch.Domain.csproj", "SimonVossSearch.Domain/"]
RUN dotnet restore "SimonVossSearch.Api/SimonVossSearch.Api.csproj"
COPY . .
WORKDIR "/src/SimonVossSearch.Api"
RUN dotnet build "SimonVossSearch.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SimonVossSearch.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimonVossSearch.Api.dll"]
