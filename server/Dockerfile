FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Yes.csproj", "./"]
RUN dotnet restore

COPY [".", "./"]
RUN dotnet build "Yes.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
RUN mkdir -p /app/Data
RUN mkdir -p /app/https
COPY ./https/aspnetcore-dev-cert.pfx /app/https/aspnetcore-dev-cert.pfx
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Yes.dll"]