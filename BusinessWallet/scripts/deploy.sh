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

# Create log directory
mkdir -p "${LOG_DIR}"

# Setup logging
TIMESTAMP=$(date '+%Y%m%d_%H%M%S')
RUNTIME_LOG="${LOG_DIR}/runtime.${TIMESTAMP}.log"
DEPLOY_LOG="${LOG_DIR}/deploy.${TIMESTAMP}.log"

exec >>"${DEPLOY_LOG}" 2>&1

log() { echo "$(date '+%F %T')  $*"; }

log "===== Starting Deployment ====="

# Go to project directory
cd "${PROJECT_DIR}"

# Kill any existing processes
log "→ Stopping any running instance..."
pkill -f "${APP_KILL_PATTERN}" 2>/dev/null || log "ℹ︎ No running instance found."
pkill -f dotnet || log "ℹ︎ No dotnet processes found."
sleep 2

# Update code
log "→ Pulling latest code..."
git fetch origin main
git reset --hard origin/main

# Fix permissions
log "→ Fixing permissions..."
chown -R $USER:$USER "${REPO_DIR}"
chmod -R u+rw "${REPO_DIR}"
chmod +x "${PROJECT_DIR}/scripts/deploy.sh"

# Clean up database and migrations
log "→ Cleaning up database and migrations..."
DB_FILE="${PROJECT_DIR}/businesswallet.db"
MIGRATIONS_DIR="${PROJECT_DIR}/Migrations"

# Remove database file and ensure it's gone
if [ -f "$DB_FILE" ]; then
    log "→ Removing database file..."
    rm -f "$DB_FILE"
    # Verify removal
    if [ -f "$DB_FILE" ]; then
        log "❌ Failed to remove database file"
        exit 1
    fi
    log "✔︎ Database file removed"
fi

# Remove migrations directory and recreate it
if [ -d "$MIGRATIONS_DIR" ]; then
    log "→ Removing migrations..."
    rm -rf "$MIGRATIONS_DIR"
    mkdir -p "$MIGRATIONS_DIR"
    log "✔︎ Migrations cleaned up"
fi

# Clean and build
log "→ Cleaning project..."
dotnet clean --configuration Release

# Create and apply migration
log "→ Creating fresh migration..."
MIGRATION_NAME="InitialCreate_$(date '+%Y%m%d_%H%M%S')"
if ! dotnet ef migrations add "${MIGRATION_NAME}" --project "${PROJECT_DIR}" --startup-project "${PROJECT_DIR}"; then
    log "❌ Failed to create migration"
    exit 1
fi
log "✔︎ Created migration: ${MIGRATION_NAME}"

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

# Start application
log "→ Starting application..."
DOTNET_ENVIRONMENT="${DOTNET_ENV}" \
nohup dotnet run --configuration Release --no-build --no-launch-profile --project "${PROJECT_DIR}/BusinessWallet.csproj" \
     > "${RUNTIME_LOG}" 2>&1 &

APP_PID=$!
log "✔︎ Application started (PID: ${APP_PID})"
log "✔︎ Runtime log: ${RUNTIME_LOG}"

log "===== Deployment Finished ====="$'\n'
