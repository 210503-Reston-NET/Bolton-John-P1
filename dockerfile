# base image
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

# mkdir and cd
WORKDIR /app

COPY *.sln ./
COPY StoreBL/*.csproj StoreBL/
COPY StoreDL/*.csproj StoreDL/
COPY StoreModels/*.csproj StoreModels/
COPY StoreWebUI/*.csproj StoreWebUI/


RUN cd StoreWebUI && dotnet restore

# Copy source code
COPY . ./
# CMD /bin/bash

RUN dotnet publish StoreWebUI -c Release -o publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime

WORKDIR /app


COPY --from=build /app/publish ./

CMD ["dotnet", "StoreWebUI.dll"]
