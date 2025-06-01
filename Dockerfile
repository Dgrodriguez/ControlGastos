FROM mcr.microsoft.com/mssql/server:2022-CU12-ubuntu-20.04
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=MiSuperClave2025!
ENV MSSQL_PID=Express

EXPOSE 1433

RUN apt-get update && apt-get install -y mssql-tools unixodbc-dev

COPY db/ControlGastos.sql /tmp/ControlGastos.sql

CMD /bin/bash -c "/opt/mssql/bin/sqlservr & sleep 20 && /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'MiSuperClave2025!' -d master -i /tmp/ControlGastos.sql && tail -f /dev/null"