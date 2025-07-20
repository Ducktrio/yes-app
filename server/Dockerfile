FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Yes.csproj", "./"]
RUN dotnet restore

COPY [".", "./"]
RUN dotnet build "Yes.csproj" -c Release -o /app


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

RUN groupadd -r app && useradd -r -g app app
RUN mkdir -p /app/Data && chmod 755 /app/Data
RUN mkdir -p /app/https && chmod 755 /app/https

COPY ./https/aspnetcore-dev-cert.pfx /app/https/aspnetcore-dev-cert.pfx
COPY --from=build /app .

RUN chown -R app:app /app
USER app

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "Yes.dll"]