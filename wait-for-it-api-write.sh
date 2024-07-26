#!/bin/bash
wait_time=30

echo "Aguardando $wait_time segundos antes de iniciar o serviço..."
sleep $wait_time

echo "Iniciando o serviço especificado..."
exec "$@"
