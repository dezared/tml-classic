#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["KeklandBankSystem/KeklandBankSystem.csproj", "KeklandBankSystem/"]
RUN dotnet restore "KeklandBankSystem/KeklandBankSystem.csproj"
COPY . .
WORKDIR "/src/KeklandBankSystem"
RUN dotnet build "KeklandBankSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KeklandBankSystem.csproj" -c Release -o /app/publish


RUN /bin/bash -c "apt install sudo"

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# set inotify and start the node application, replace yar with your command
RUN echo "#!/bin/sh \n\
echo "fs.inotify.max_user_watches before update" \n\
cat /etc/sysctl.conf\n\
echo "______________________________________________updating inotify ____________________________________" \n\
echo fs.inotify.max_user_watches=524288 | tee -a /etc/sysctl.conf && sysctl -p \n\
echo "updated value is" \n\
cat /etc/sysctl.conf | grep fs.inotify \n\
exec yarn start:dev \
" >> /usr/local/bin/entrypoint.sh

RUN chmod +x /usr/local/bin/entrypoint.sh


EXPOSE 80
ENTRYPOINT ["dotnet", "KeklandBankSystem.dll"]
