#!/usr/bin/env bash

## Check if we already have env file
if [ -f Secrets/db.env ]; then
	echo "env already setup"
	exit 0
fi

DB_PASSWORD=$(aws secretsmanager get-random-password --exclude-punctuation --password-length=30 | jq -r '.RandomPassword')
echo "POSTGRES_PASSWORD=${DB_PASSWORD}" > Secrets/db.env

dotnet user-secrets set "DB:ConnectionString" "host=127.0.0.1;port=5432;database=postgres;username=postgres;password=${DB_PASSWORD}"
