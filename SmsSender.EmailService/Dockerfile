#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG GITHUB_USERNAME
ARG GITHUB_PASSWORD
WORKDIR /src
COPY ["SmsSender.EmailService/SmsSender.EmailService.csproj", "SmsSender.EmailService/"]
RUN dotnet nuget add source --username $GITHUB_USERNAME --password $GITHUB_PASSWORD --store-password-in-clear-text --name github "https://nuget.pkg.github.com/avokado03/index.json"
RUN dotnet restore "SmsSender.EmailService/SmsSender.EmailService.csproj"
COPY . .
WORKDIR "/src/SmsSender.EmailService"
RUN dotnet build "SmsSender.EmailService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmsSender.EmailService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmsSender.EmailService.dll"]