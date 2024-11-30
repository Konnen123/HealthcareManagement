# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the entire solution, including .csproj files (not just published output)
COPY . ./

# Restore the dependencies for the entire solution
RUN dotnet restore ./HealthcareManagement/HealthcareManagement.csproj

# Apply migrations using the Infrastructure project (point to .csproj)
RUN dotnet tool install --global dotnet-ef --version 9.0.0
ENV PATH="$PATH:/root/.dotnet/tools"

# Apply migrations using dotnet-ef
RUN dotnet ef database update --project ./HealthcareManagement/HealthcareManagement.csproj

# Publish the application from the HealthcareManagement project
RUN dotnet publish ./HealthcareManagement/HealthcareManagement.csproj -c Release -o /app/publish

# Use the official .NET Runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# Set the working directory for the app
WORKDIR /app

# Copy the published files from the build stage
COPY --from=build /app/publish .

# Ensure the global tools are available in the runtime stage
ENV PATH="${PATH}:/root/.dotnet/tools"

# Expose the port the app will run on
EXPOSE 80

# Define the entry point to run the app
#ENTRYPOINT ["dotnet", "HealthcareManagement.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet HealthcareManagement.dll
