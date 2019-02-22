FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app
# Copy everything
COPY . ./
RUN ls /app
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/CTM.QuoteAPI/out .
ENTRYPOINT ["dotnet", "CTM.QuoteAPI.dll"]
