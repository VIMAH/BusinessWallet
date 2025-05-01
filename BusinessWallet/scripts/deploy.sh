#!/usr/bin/env bash
#test

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
pkill -f dotnet || log "ℹ︎ No dotnet processes found."

# Update code
log "→ Pulling latest code..."
git fetch origin main
git reset --hard origin/main

log "→ Fixing permissions..."
chown -R $USER:$USER "${REPO_DIR}"
chmod -R u+rw "${REPO_DIR}"
chmod +x "${PROJECT_DIR}/scripts/deploy.sh"

# Remove existing database and migrations
log "→ Cleaning up database and migrations..."
if [ -f "${PROJECT_DIR}/businesswallet.db" ]; then
    rm -f "${PROJECT_DIR}/businesswallet.db"
    log "✔︎ Removed existing database"
fi

if [ -d "${PROJECT_DIR}/Migrations" ]; then
    rm -rf "${PROJECT_DIR}/Migrations"
    mkdir -p "${PROJECT_DIR}/Migrations"
    log "✔︎ Removed existing migrations"
fi

# Build clean
log "→ Cleaning project..."
dotnet clean --configuration Release

# Create fresh migration
log "→ Creating fresh migration..."
MIGRATION_NAME="InitialCreate_$(date '+%Y%m%d_%H%M%S')"
if ! dotnet ef migrations add "${MIGRATION_NAME}" --project "${PROJECT_DIR}" --startup-project "${PROJECT_DIR}"; then
    log "❌ Failed to create migration"
    exit 1
fi
log "✔︎ Created fresh migration: ${MIGRATION_NAME}"

# Update database
log "→ Applying database migrations..."
if ! dotnet ef database update --project "${PROJECT_DIR}" --startup-project "${PROJECT_DIR}"; then
    log "❌ Migration failed"
    exit 1
fi
log "✔︎ Database migrations applied successfully"

# Build project
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
