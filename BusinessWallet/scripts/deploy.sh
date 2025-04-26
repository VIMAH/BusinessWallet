#!/usr/bin/env bash

# Simple deploy script for BusinessWallet

set -euo pipefail

# ─── Configuration ─────────────────────────────────────────────────
REPO_DIR="/root/businesswalletapi"
PROJECT_DIR="${REPO_DIR}/BusinessWallet"
LOG_DIR="/var/log/businesswallet"
APP_KILL_PATTERN="BusinessWallet.dll"
DOTNET_ENV="Production"
# ───────────────────────────────────────────────────────────────────

mkdir -p "${LOG_DIR}"

TIMESTAMP=$(date '+%Y%m%d_%H%M%S')
RUNTIME_LOG="${LOG_DIR}/runtime.${TIMESTAMP}.log"
DEPLOY_LOG="${LOG_DIR}/deploy.${TIMESTAMP}.log"

exec >>"${DEPLOY_LOG}" 2>&1

log() { echo "$(date '+%F %T')  $*"; }

log "===== Starting Deployment ====="

# Go to project
cd "${PROJECT_DIR}"

# Kill any existing app
log "→ Stopping any running instance..."
pkill -f "${APP_KILL_PATTERN}" 2>/dev/null || log "ℹ︎ No running instance found."

# Update code
log "→ Pulling latest code..."
git fetch origin main
git reset --hard origin/main

# Build clean
log "→ Cleaning project..."
dotnet clean --configuration Release

log "→ Updating database (if migrations exist)..."
if [ -d "${PROJECT_DIR}/Migrations" ]; then
  dotnet ef database update --project "${PROJECT_DIR}"
else
  log "ℹ︎ No migrations found, skipping database update."
fi

log "→ Building project..."
dotnet build --configuration Release --no-restore

# Start application in background
log "→ Starting application in background..."
DOTNET_ENVIRONMENT="${DOTNET_ENV}" \
nohup dotnet run --configuration Release --no-build --no-launch-profile --project "${PROJECT_DIR}/BusinessWallet.csproj" \
     > "${RUNTIME_LOG}" 2>&1 &

APP_PID=$!
log "✔︎ Application started (PID: ${APP_PID})"
log "✔︎ Runtime log: ${RUNTIME_LOG}"

log "===== Deployment Finished ====="$'\n'
