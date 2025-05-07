FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ArrayPoolz.csproj", "./"]
RUN dotnet restore "ArrayPoolz.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "ArrayPoolz.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ArrayPoolz.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ArrayPoolz.dll"]
