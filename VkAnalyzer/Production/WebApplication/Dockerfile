FROM microsoft/dotnet:2.2-sdk
COPY . /app
WORKDIR /app/WebApplication
RUN dotnet restore
RUN dotnet publish -c Release
EXPOSE 5555/tcp
RUN chmod +x entrypoint.sh
CMD /bin/bash entrypoint.sh