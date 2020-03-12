SONARCLOUD_KEY=myexpenses-backend
SONARCLOUD_ORG=lfmachadodasilva-github
SONARCLOUD_TOKEN=e89e5da32d3180c75af1472e1cb4cfbafdd52579

dotnet sonarscanner begin /key:$SONARCLOUD_KEY /organization:$SONARCLOUD_ORG /d:sonar.host.url="https://sonarcloud.io" /d:sonar.login=$SONARCLOUD_TOKEN /d:sonar.cs.opencover.reportsPaths="Coverage\*.xml" 