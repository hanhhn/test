FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim

RUN apt-get update && apt-get -y install libgdiplus dnsutils iputils-ping curl

RUN cd /usr/lib && ln -s libgdiplus.so gdiplus.dll

WORKDIR /apps

FROM mcr.microsoft.com/dotnet/runtime:3.1-buster-slim AS base
WORKDIR /app
RUN apt-get update && apt-get install -y libfontconfig1

FROM mcr.microsoft.com/dotnet/sdk:3.1-buster AS build
WORKDIR /src
COPY ["TestNet.csproj", "."]
RUN dotnet restore "./TestNet.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "TestNet.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestNet.csproj" -c Release -r linux-x64 --no-self-contained -o /app/publish

FROM base AS final

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "aspose.words.linux.dll"]