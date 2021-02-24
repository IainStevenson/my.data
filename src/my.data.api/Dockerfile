#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["my.data.api/my.data.api.csproj", "my.data.api/"]
RUN dotnet restore "my.data.api/my.data.api.csproj"
COPY . .
WORKDIR "/src/my.data.api"
RUN dotnet build "my.data.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "my.data.api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "my.data.api.dll"]
ENTRYPOINT ["bash", "/usr/local/bin/bash"]