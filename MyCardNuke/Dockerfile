FROM microsoft/aspnetcore-build AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.1 AS build
WORKDIR /src
COPY MyCardNuke.sln ./
COPY MyCardNuke/MyCardNuke.csproj MyCardNuke/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/MyCardNuke
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MyCardNuke.dll"]
