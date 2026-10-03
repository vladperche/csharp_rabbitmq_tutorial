# BUILD
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY ./myRabbitProducer.sln ./
COPY ./myRabbitProducer.API/myRabbitProducer.API.csproj myRabbitProducer.API/
COPY ./myRabbitProducer.Entities/myRabbitProducer.Entities.csproj myRabbitProducer.Entities/
COPY ./myRabbitProducer.Services/myRabbitProducer.Services.csproj myRabbitProducer.Services/
COPY ./myRabbitProducer.CrossCutting/myRabbitProducer.CrossCutting.csproj myRabbitProducer.CrossCutting/

RUN dotnet restore myRabbitProducer.API/myRabbitProducer.API.csproj

COPY ./ .

RUN dotnet publish myRabbitProducer.API/myRabbitProducer.API.csproj -c Release -o /app/publish

# RUNTIME
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV DOTNET_ENVIRONMENT=DockerTest

RUN mkdir -p /app/logs

VOLUME ["/app/logs"]

EXPOSE 7133
EXPOSE 5169
ENTRYPOINT ["dotnet", "myRabbitProducer.API.dll"]
