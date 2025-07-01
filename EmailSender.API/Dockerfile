FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["EmailSender.API/EmailSender.API.csproj", "EmailSender.API/"]
COPY ["EmailSender.Infrastructure/EmailSender.Infrastructure.csproj", "EmailSender.Infrastructure/"]
COPY ["EmailSender.Domain/EmailSender.Domain.csproj", "EmailSender.Domain/"]
COPY ["EmailSender.Shared/EmailSender.Shared.csproj", "EmailSender.Shared/"]
COPY ["EmailSender.Core/EmailSender.Core.csproj", "EmailSender.Core/"]

RUN dotnet restore "EmailSender.API/EmailSender.API.csproj"

COPY . .
WORKDIR "/src/EmailSender.API"
RUN dotnet publish "EmailSender.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EmailSender.API.dll"]