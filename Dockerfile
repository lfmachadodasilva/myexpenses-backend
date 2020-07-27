################
## Base Image ##
################
FROM images.artifactory.dunnhumby.com/dunnhumby/aspnetcore-runtime:3.1 AS base
#FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base

#############
## Restore ##
#############
FROM images.artifactory.dunnhumby.com/dunnhumby/aspnetcore-build:3.1 AS restore
#FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS restore

WORKDIR /app
EXPOSE 80

RUN mkdir ./MyExpenses
RUN mkdir ./MyExpenses.UnitTests

COPY MyExpenses/*.csproj ./MyExpenses
COPY MyExpenses.UnitTests/*.csproj ./MyExpenses.UnitTests
COPY MyExpenses.sln .

RUN ["dotnet", "restore"]

###########
## Build ##
###########
FROM restore AS build

COPY MyExpenses ./MyExpenses
COPY MyExpenses.UnitTests ./MyExpenses.UnitTests
COPY MyExpenses.sln .

RUN ["dotnet", "publish", "--no-restore", "-c", "Release"]

################
## Unit Tests ##
################
FROM restore AS test
COPY . .
VOLUME ["/app/coverage"]
ENTRYPOINT ["dotnet", "test", "-c", "Release", "--collect:\"XPlat Code Coverage\""]

###################
## Release Image ##
###################
FROM build AS final
WORKDIR /app
COPY --from=build /app/MyExpenses/bin/Release/*/publish .
ENTRYPOINT ["dotnet", "MyExpenses.dll"]
