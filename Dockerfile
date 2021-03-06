#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["accepted.csproj", "."]
RUN dotnet restore "accepted.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "accepted.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "accepted.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY wait-for-it.sh wait-for-it.sh
RUN chmod +x wait-for-it.sh

EXPOSE 5001
#ENTRYPOINT ["dotnet", "Accepted.dll"]

