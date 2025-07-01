FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["EmailSender.Worker/EmailSender.Worker.csproj", "EmailSender.Worker/"]
COPY ["EmailSender.Infrastructure/EmailSender.Infrastructure.csproj", "EmailSender.Infrastructure/"]
COPY ["EmailSender.Domain/EmailSender.Domain.csproj", "EmailSender.Domain/"]
COPY ["EmailSender.Shared/EmailSender.Shared.csproj", "EmailSender.Shared/"]
COPY ["EmailSender.Core/EmailSender.Core.csproj", "EmailSender.Core/"]

RUN dotnet restore "EmailSender.Worker/EmailSender.Worker.csproj"

COPY . .
WORKDIR "/src/EmailSender.Worker"
RUN dotnet publish "EmailSender.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EmailSender.Worker.dll"]