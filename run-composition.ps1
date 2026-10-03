
[CmdletBinding()]
param(
    [switch]$BuildOnly,
    [switch]$FollowLogs,
    [switch]$Down
)

$workspaceRoot = $PSScriptRoot
Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "  RabbitMQ Example - Docker Composition Runner" -ForegroundColor Cyan
Write-Host "  Workspace Root: $workspaceRoot" -ForegroundColor Yellow
Write-Host "==========================================================" -ForegroundColor Cyan

New-Item -ItemType Directory -Path "dockerdata\rabbitmq" -Force
New-Item -ItemType Directory -Path "dockerdata\producer" -Force
New-Item -ItemType Directory -Path "dockerdata\consumer" -Force

$env:workspaceFolder = $workspaceRoot.Replace("\", "/")
$env:WORKSPACE_FOLDER = $env:workspaceFolder

if ($Down) {
    Write-Host "Stopping and tearing down docker composition..." -ForegroundColor Yellow
    docker compose down -v
    Write-Host "Tear down complete." -ForegroundColor Green
    exit 0
}

if ($BuildOnly) {
    Write-Host "Building Docker images for Producer and Consumer..." -ForegroundColor Cyan
    docker compose build
    Write-Host "Build complete." -ForegroundColor Green
    exit 0
}

Write-Host "Starting Docker containers with build..." -ForegroundColor Cyan
docker compose up --build -d

Write-Host ""
Write-Host "Containers Status:" -ForegroundColor Green
docker compose ps
