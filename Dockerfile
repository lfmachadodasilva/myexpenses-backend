#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["MyExpenses/MyExpenses.csproj", "MyExpenses/"]
RUN dotnet restore "MyExpenses/MyExpenses.csproj"
COPY . .
WORKDIR "/src/MyExpenses"
RUN dotnet build "MyExpenses.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyExpenses.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyExpenses.dll"]