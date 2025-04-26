#!/usr/bin/env bash
#
# deploy.sh – BusinessWallet (zonder Docker)
# ------------------------------------------------------------------
# Doet alleen iets als er nieuwe commits op origin/main staan:
#   1. Haalt laatste code op
#   2. Stopt draaiende app
#   3. dotnet clean            → ruimt oude artefacts op
#   4. dotnet ef database update  (indien Migrations-map bestaat)
#   5. dotnet build            → Release-build
#   6. dotnet run              → start in achtergrond
# Elke stap (en fouten) wordt gelogd in /var/log/businesswallet/*.log
# ------------------------------------------------------------------

set -euo pipefail

# ───── Configuratie ───────────────────────────────────────────────
REPO_DIR="/home/ubuntu/BusinessWallet"          # root van je repo
PROJECT_DIR="${REPO_DIR}/BusinessWallet"        # submap met .csproj
LOG_DIR="/var/log/businesswallet"               # map voor logs
DOTNET_ENV="Production"                         # of Development
APP_KILL_PATTERN="BusinessWallet.dll"           # pkill-pattern
# ──────────────────────────────────────────────────────────────────

mkdir -p "${LOG_DIR}"
DEPLOY_LOG="${LOG_DIR}/deploy.$(date '+%Y%m%d').log"

# Alles wat echo't → deploy-log
exec >>"${DEPLOY_LOG}" 2>&1

log() { echo "$(date '+%F %T')  $*"; }
trap 'log "❌ Deployment FAILED (exit code: $?)"' ERR

log "========== DEPLOYMENT TRIGGERED =========="

cd "${REPO_DIR}" || { log "❌  ${REPO_DIR} bestaat niet"; exit 1; }

log "→ Fetching origin/main..."
git fetch origin main

LOCAL_COMMIT=$(git rev-parse HEAD)
REMOTE_COMMIT=$(git rev-parse origin/main)

if [[ "${LOCAL_COMMIT}" == "${REMOTE_COMMIT}" ]]; then
  log "✔︎ Geen nieuwe commits – deployment gestopt."
  exit 0
fi

log "✎ Nieuwe commits gevonden (${LOCAL_COMMIT:0:7} → ${REMOTE_COMMIT:0:7}). Updating..."
git reset --hard origin/main

log "→ Stopping huidige instantie (pattern: ${APP_KILL_PATTERN})..."
pkill -f "${APP_KILL_PATTERN}" 2>/dev/null || log "ℹ︎ Geen draaiende instantie gevonden."

cd "${PROJECT_DIR}"

log "→ dotnet clean..."
dotnet clean --configuration Release

log "→ dotnet ef database update (indien migrations)..."
if [ -d "${PROJECT_DIR}/Migrations" ]; then
  dotnet ef database update --project "${PROJECT_DIR}"
else
  log "ℹ︎ Geen Migrations-map gevonden – stap overgeslagen."
fi

log "→ dotnet build..."
dotnet build --configuration Release --no-restore

log "→ Starting application met dotnet run..."
RUNTIME_LOG="${LOG_DIR}/runtime.$(date '+%Y%m%d_%H%M%S').log"
DOTNET_ENVIRONMENT="${DOTNET_ENV}" \
nohup dotnet run --configuration Release --no-build --no-launch-profile \
     > "${RUNTIME_LOG}" 2>&1 &

APP_PID=$!
log "✔︎ Applicatie gestart (PID ${APP_PID}). Runtime-log: ${RUNTIME_LOG}"
log "========== DEPLOYMENT FINISHED ==========="$'\n'
