################
## Base Image ##
################
#FROM images.artifactory.dunnhumby.com/dunnhumby/aspnetcore-runtime:3.1 AS base
FROM microsoft/aspnetcore-runtime:3.1 AS base

WORKDIR /app
EXPOSE 80

#############
## Restore ##
#############
#FROM images.artifactory.dunnhumby.com/dunnhumby/aspnetcore-build:3.1 AS restore
FROM microsoft/aspnetcore-runtime:3.1 AS restore

WORKDIR /app
EXPOSE 80

COPY . .
RUN ["dotnet", "restore"]

###########
## Build ##
###########
FROM restore AS build
RUN ["dotnet", "publish", "--no-restore", "-c", "Release"]

################
## Unit Tests ##
################
FROM restore AS test
ENTRYPOINT ["dotnet", "test", "--no-build", "-c", "Release"]

###################
## Release Image ##
###################
FROM base AS final
WORKDIR /app
COPY --from=build /app/MyExpenses/bin/Release/*/publish .
ENTRYPOINT ["dotnet", "MyExpenses.dll"]