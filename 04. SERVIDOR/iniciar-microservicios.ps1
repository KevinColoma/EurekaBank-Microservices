# Inicia todos los microservicios de EurekaBank en ventanas separadas.
# Uso: .\iniciar-microservicios.ps1

$base = $PSScriptRoot

$servicios = @(
    @{ Nombre = "Auth.Api (5171)";          Ruta = "Auth.Api" },
    @{ Nombre = "Movimientos.Api (5172)";   Ruta = "Movimientos.Api" },
    @{ Nombre = "Operaciones.Api (5173)";   Ruta = "Operaciones.Api" },
    @{ Nombre = "Transferencia.Api (5174)"; Ruta = "Transferencia.Api" },
    @{ Nombre = "Gateway.Api (5069)";       Ruta = "Gateway.Api" }
)

foreach ($s in $servicios) {
    $proyecto = Join-Path $base $s.Ruta
    Write-Host "Iniciando $($s.Nombre)..." -ForegroundColor Cyan
    Start-Process powershell -ArgumentList "-NoExit", "-Command", "`$Host.UI.RawUI.WindowTitle = '$($s.Nombre)'; dotnet run --project `"$proyecto`""
}

Write-Host ""
Write-Host "Todos los servicios iniciados. El Gateway escucha en http://localhost:5069 (mismas rutas /Eureka/* que el monolito)." -ForegroundColor Green
