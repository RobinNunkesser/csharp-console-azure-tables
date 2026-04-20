#!/usr/bin/env bash
# teardown.sh – Azure-Ressourcen vollständig löschen
set -euo pipefail

RESOURCE_GROUP="TodoRG"

echo "Lösche Ressourcengruppe '$RESOURCE_GROUP' und alle enthaltenen Ressourcen ..."
az group delete --name "$RESOURCE_GROUP" --yes --no-wait
echo "Löschvorgang gestartet (läuft im Hintergrund)."
