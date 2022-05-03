#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80/tcp

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SenderApp/SenderApp.csproj", "SenderApp/"]
COPY ["NugetPackages.Serilog/NugetPackages.Serilog.csproj", "NugetPackages.Serilog/"]
COPY ["NugetPackages.Mapper/NugetPackages.Mapper.csproj", "NugetPackages.Mapper/"]
COPY ["Core.AppIdentity/Core.AppIdentity.csproj", "Core.AppIdentity/"]
COPY ["NugetPackages.Identity/NugetPackages.Identity.csproj", "NugetPackages.Identity/"]
COPY ["NugetPackages.RazorEngine.Template/NugetPackages.RazorEngine.Template.csproj", "NugetPackages.RazorEngine.Template/"]
COPY ["NugetPackages.Quartz/NugetPackages.Quartz.csproj", "NugetPackages.Quartz/"]
COPY ["Core.Abstractions/Core.Abstractions.csproj", "Core.Abstractions/"]
COPY ["NugetPackages.MailKit/NugetPackages.MailKit.csproj", "NugetPackages.MailKit/"]
COPY ["NugetPackages.MySqlProvider/NugetPackages.MySqlProvider.csproj", "NugetPackages.MySqlProvider/"]
RUN dotnet restore "SenderApp/SenderApp.csproj"
COPY . .
WORKDIR "/src/SenderApp"
RUN dotnet build "SenderApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SenderApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SenderApp.dll"]