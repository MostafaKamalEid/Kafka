FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Kafka/Kafka.csproj", "Kafka/"]
RUN dotnet restore "Kafka/Kafka.csproj"
COPY . .
WORKDIR "/src/Kafka"
RUN dotnet build "Kafka.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kafka.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kafka.dll"]