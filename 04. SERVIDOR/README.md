# EurekaBank — Arquitectura de Microservicios

Migración del servidor monolítico `WS_EUREKANUBE_RESTFULL` a microservicios, **sin cambiar el contrato HTTP**: los clientes (CLICON, CLIESC, CLIWEB, CLIMOVIL) siguen apuntando a `http://<host>:5069/Eureka/...` sin ninguna modificación.

## Arquitectura

```
CLIENTES (consola, escritorio, web, móvil)
        │  http://<host>:5069/Eureka/...   (sin cambios)
        ▼
┌─────────────────────────────────────────────┐
│  Gateway.Api  (YARP)          puerto 5069   │
└─────────────────────────────────────────────┘
   │              │               │              │
   ▼              ▼               ▼              ▼
Auth.Api     Movimientos.Api  Operaciones.Api  Transferencia.Api
(5171)       (5172)           (5173)           (5174)
validar-     movimientos/     deposito         transferencia
usuario      {cuenta}         retiro
   │              │               │              │
   └──────────────┴───────┬───────┴──────────────┘
                          ▼
        SQL Server (AWS RDS) — EUREKABANK
```

## Servicios

| Servicio | Puerto | Responsabilidad | Endpoints |
|---|---|---|---|
| `Gateway.Api` | **5069** | Punto de entrada único (YARP reverse proxy) | Enruta todo `/Eureka/*` |
| `Auth.Api` | 5171 | Login / validación de credenciales (SHA-256) | `POST /Eureka/validar-usuario` |
| `Movimientos.Api` | 5172 | Consulta de movimientos de una cuenta | `GET /Eureka/movimientos/{cuenta}` |
| `Operaciones.Api` | 5173 | Depósitos y retiros (transacción Serializable) | `POST /Eureka/deposito`, `POST /Eureka/retiro` |
| `Transferencia.Api` | 5174 | Transferencia entre cuentas (transacción Serializable) | `POST /Eureka/transferencia` |

Cada microservicio conserva la misma lógica de negocio, SQL, códigos de estado HTTP y mensajes del monolito. Todos comparten la misma base de datos SQL Server en AWS RDS (patrón *shared database*, primer paso típico de una migración; a futuro puede separarse por esquema/BD por servicio).

## Cómo ejecutar

Requisito: .NET 8 SDK (o superior).

```powershell
cd "04. SERVIDOR"
.\iniciar-microservicios.ps1
```

Esto abre 5 ventanas (4 microservicios + gateway). También puedes iniciar cada uno manualmente:

```powershell
dotnet run --project .\Auth.Api
dotnet run --project .\Movimientos.Api
dotnet run --project .\Operaciones.Api
dotnet run --project .\Transferencia.Api
dotnet run --project .\Gateway.Api
```

Compilar todo:

```powershell
dotnet build .\Eureka.Microservicios.sln
```

## Pruebas rápidas (contra el gateway)

```powershell
# Login
curl.exe -X POST "http://localhost:5069/Eureka/validar-usuario?usuario=USUARIO&password=CLAVE"

# Movimientos
curl.exe "http://localhost:5069/Eureka/movimientos/00100001"

# Depósito
curl.exe -X POST "http://localhost:5069/Eureka/deposito?cuenta=00100001&importe=50"

# Retiro
curl.exe -X POST "http://localhost:5069/Eureka/retiro?cuenta=00100001&importe=20"

# Transferencia
curl.exe -X POST "http://localhost:5069/Eureka/transferencia?cuentaOrigen=00100001&cuentaDestino=00100002&importe=10"
```

## Clientes

**No requieren cambios.** El gateway escucha en el mismo puerto (5069) y expone exactamente las mismas rutas que el monolito. Solo asegúrate de que la IP configurada en cada cliente apunte a la máquina donde corre el gateway (igual que antes con el monolito).

- CLICON: `ec.edu.monster.modelo/EurekabankService.cs` → `http://localhost:5069/Eureka`
- CLIESC: `ec.edu.monster.service/EurekaService.cs` → `http://localhost:5069/Eureka/`
- CLIWEB: `appsettings.json` → `ApiEureka:UrlBase`
- CLIMOVIL: `modelo/Config.java` → puerto 5069

## Estructura de cada microservicio

Se mantiene el mismo estilo del monolito:

```
<Servicio>.Api/
├── Controllers/EurekaController.cs   # Solo los endpoints de su dominio (ruta base /Eureka)
├── Servicio/<X>Servicio.cs           # Lógica de negocio copiada del monolito
├── AccesoDB/AccesoDB.cs              # Conexión a SQL Server
├── Model/                            # (solo Movimientos.Api)
├── Program.cs
└── appsettings.json                  # Puerto Kestrel del servicio
```
