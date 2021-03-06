FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

COPY *.sln .
COPY AirTNG.Web/*.csproj ./AirTNG.Web/
COPY AirTNG.Web.Tests/*.csproj ./AirTNG.Web.Tests/
RUN dotnet restore

COPY . .
WORKDIR /source/AirTNG.Web

RUN dotnet tool install --global dotnet-ef --version 3.0.0
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet ef database update

RUN dotnet build

EXPOSE 5000

ENTRYPOINT [ "dotnet", "run" ]
