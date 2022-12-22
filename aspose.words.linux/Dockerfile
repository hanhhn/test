FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS build-env
WORKDIR /app
COPY . ./
RUN dotnet restore "aspose.words.linux.csproj"
RUN dotnet build "aspose.words.linux.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim
WORKDIR /app
COPY --from=build-env /app/out .


ENTRYPOINT ["dotnet", "aspose.words.linux.dll"]