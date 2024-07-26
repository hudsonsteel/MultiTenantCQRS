#!/bin/bash

wait_time=1s
max_retries=5
password="IndySolft&Password1"

# Função para executar o script SQL
function run_sql_script {
  echo "Executando script SQL..."
  /opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P $password -i /tmp/init.sql
  return $?
}

# Espera para o SQL Server estar disponível
echo "A importação de dados começará em $wait_time..."
sleep $wait_time

# Tentativas de executar o script SQL
attempt=1
while [ $attempt -le $max_retries ]; do
  run_sql_script
  result=$?

  if [ $result -eq 0 ]; then
    echo "Script SQL executado com sucesso!"
    break
  else
    echo "Falha ao executar o script SQL. Tentativa $attempt de $max_retries. Aguardando $wait_time antes de tentar novamente..."
    sleep $wait_time
  fi

  attempt=$((attempt + 1))
done

if [ $result -ne 0 ]; then
  echo "Falha ao executar o script SQL após $max_retries tentativas. Verifique os logs para mais detalhes."
  exit 1
fi

# Inicia o serviço especificado (caso exista algum)
exec "$@"
