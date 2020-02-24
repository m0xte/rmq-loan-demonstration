FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy everything else and build
COPY . ./
RUN cd CTM.QuoteProviderB && dotnet restore
RUN cd CTM.QuoteProviderB && dotnet publish -c Release -o ../out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "CTM.QuoteProviderB.dll"]
