FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["WebApiVotacion/WebApiVotacion.csproj", "WebApiVotacion/"]
RUN dotnet restore "WebApiVotacion/WebApiVotacion.csproj"
COPY . .
WORKDIR "/src/WebApiVotacion"
RUN dotnet build "WebApiVotacion.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "WebApiVotacion.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebApiVotacion.dll"]