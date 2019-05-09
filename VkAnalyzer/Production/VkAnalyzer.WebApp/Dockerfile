FROM microsoft/dotnet:2.2-sdk
COPY . /app
WORKDIR /app/VkAnalyzer.WebApp
RUN dotnet restore
RUN dotnet publish -c Release
EXPOSE 5555/tcp
# RUN chmod +x entrypoint.sh
# CMD /bin/bash entrypoint.sh
ENTRYPOINT [ "dotnet", "run", "--server.urls http://*:5555" ]