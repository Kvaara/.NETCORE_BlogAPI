#!/bin/bash

# Stop the execution if there are any runtime errors
set -e

POSTGRES="psql --username ${POSTGRES_USER}"

echo "Creating Blog API PostgreSQL Database..."

# EOSQL is a way of creating a "here document"
# We can use this "here document" code block to write multiple commands after a certain command
# such as $POSTGRES above or cat
$POSTGRES << EOSQL
CREATE DATABASE ${POSTGRES_DATABASE} OWNER ${POSTGRES_USER};
EOSQL

