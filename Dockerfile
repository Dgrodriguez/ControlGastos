FROM mcr.microsoft.com/mssql/server:2022-CU12-ubuntu-20.04
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=MiSuperClave2025!
ENV MSSQL_PID=Express

EXPOSE 1433

RUN mkdir -p /var/lib/apt/lists/partial && chmod -R 777 /var/lib/apt/lists/
RUN apt-get update && apt-get install -y curl sudo \
    && sudo curl -O https://packages.microsoft.com/keys/microsoft.asc \
    && sudo apt-key add microsoft.asc \
    && sudo add-apt-repository "$(curl -s https://packages.microsoft.com/config/ubuntu/20.04/mssql-server-2019.list)" \
    && sudo apt-get update \
    && sudo apt-get install -y mssql-tools unixodbc-dev

COPY DB/ControlGastos.sql /tmp/ControlGastos.sql

CMD /bin/bash -c "/opt/mssql/bin/sqlservr & sleep 20 && /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'MiSuperClave2025!' -d master -i /tmp/ControlGastos.sql && tail -f /dev/null"