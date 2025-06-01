FROM mcr.microsoft.com/mssql/server:2022-latest  # Última versión estable

ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=MiSuperClave2025!
ENV MSSQL_PID=Express

EXPOSE 1433

# Copia el script SQL dentro del contenedor
COPY db/init.sql /tmp/init.sql

# Ejecuta el script cuando el contenedor inicie
RUN /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "MiSuperClave2025!" -d master -i /tmp/init.sql

CMD ["/opt/mssql/bin/sqlservr"]