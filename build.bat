docker create --rm -it --name builder mcr.microsoft.com/dotnet/core/sdk:3.0
docker start builder

docker cp src/. builder:/src
docker exec -w /src builder dotnet publish -c Release -o /out -r linux-x64 /p:PublishReadyToRun=true
docker exec -w /out builder chmod +x HelloWorld

docker exec builder apt-get -qq update
docker exec builder apt-get -qq install -y --no-install-recommends zip
docker exec builder mkdir -p /dist
docker exec -w /out builder sh -c "zip -r /dist/package.zip *"

docker cp builder:/dist/. dist

docker stop builder
