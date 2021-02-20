#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["My.Data.Bandwidth/My.Data.Bandwidth.csproj", "My.Data.Bandwidth/"]
RUN dotnet restore "My.Data.Bandwidth/My.Data.Bandwidth.csproj"
COPY . .
WORKDIR "/src/My.Data.Bandwidth"
RUN dotnet build "My.Data.Bandwidth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "My.Data.Bandwidth.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "My.Data.Bandwidth.dll"]
