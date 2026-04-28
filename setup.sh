#!/usr/bin/env bash
# setup.sh – Azure Table Storage einrichten (komplett per CLI)
# Voraussetzungen: az CLI installiert, az login ausgeführt
set -euo pipefail

RESOURCE_GROUP="TodoRG"
LOCATION="germanywestcentral"
# Storage-Account-Name muss global eindeutig sein (3–24 Zeichen, nur Kleinbuchstaben/Zahlen)
STORAGE_ACCOUNT="todostorage$(date +%s | tail -c 7)"

echo "=== Azure Table Storage Setup ==="
echo "Resource Group : $RESOURCE_GROUP"
echo "Location       : $LOCATION"
echo "Storage Account: $STORAGE_ACCOUNT"
echo ""

# 1. Ressourcengruppe anlegen
az group create \
  --name "$RESOURCE_GROUP" \
  --location "$LOCATION" \
  --output table

# 2. Storage Account anlegen (Standard_LRS = günstigste Redundanz, Free-Tier-kompatibel)
az storage account create \
  --name "$STORAGE_ACCOUNT" \
  --resource-group "$RESOURCE_GROUP" \
  --sku Standard_LRS \
  --kind StorageV2 \
  --output table

# 3. Connection String auslesen
CONNECTION_STRING=$(az storage account show-connection-string \
  --name "$STORAGE_ACCOUNT" \
  --resource-group "$RESOURCE_GROUP" \
  --query connectionString \
  --output tsv)

echo ""
echo "=== Umgebungsvariable setzen ==="
echo "export AZURE_STORAGE_CONNECTION_STRING='$CONNECTION_STRING'"
echo ""
echo "Dann starten mit: dotnet run --project AzureTables"
