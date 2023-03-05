FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5044

ENV ASPNETCORE_URLS=http://+:5044

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BeeFriends.csproj", "./"]
RUN dotnet restore "BeeFriends.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "BeeFriends.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BeeFriends.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BeeFriends.dll"]
