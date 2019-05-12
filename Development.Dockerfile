FROM mcr.microsoft.com/dotnet/core/sdk:2.2
WORKDIR /code/app
ENV ASPNETCORE_ENVIRONMENT=Development
COPY *.csproj ./
RUN dotnet restore
COPY . ./

ENTRYPOINT [ "dotnet", "watch", "run" ]