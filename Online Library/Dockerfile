#First-Stage base image

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

#COpy csproj and get all the depenedencies
COPY *.csproj .
RUN dotnet restore

# copy and publich application files
COPY . .
RUN dotnet publish -c release -o /app 

#final stage image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .


ENTRYPOINT [ "dotnet", "Online Library.dll" ]

