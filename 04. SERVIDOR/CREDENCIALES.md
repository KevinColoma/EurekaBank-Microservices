# Credenciales de Prueba — EurekaBank

## Usuario de la Aplicación

Para probar los clientes (CLICON, CLIESC, CLIWEB, CLIMOVIL):

```
Usuario:     Monster
Contraseña:  Monster9
```

Esta cuenta está registrada en la tabla `auth` de la base de datos y tiene permisos para realizar operaciones (consultar movimientos, depósitos, retiros, transferencias).

## Conexión a la Base de Datos

Las credenciales de administrador de la BD están en el archivo `.env` (no versionado):

```bash
# .env (crear en la raíz de esta carpeta si no existe)
EUREKA_DB_CONNECTION=Server=eurekanetaws.c3gqau2qudkh.sa-east-1.rds.amazonaws.com,1433; Database=eurekanetawsrest; User Id=admin; Password=31Diavl0123; TrustServerCertificate=True; Encrypt=True;
```

O copia desde `.env.example` y completa tu contraseña.

## Cómo probar los clientes

### 1. Iniciar los microservicios

```powershell
cd "04. SERVIDOR"
.\iniciar-microservicios.ps1
```

Esto abre 5 ventanas (Gateway + 4 microservicios). El Gateway escucha en **puerto 5069**.

### 2. Probar desde un cliente

Usa las credenciales arriba:

```bash
# CLICON (consola)
dotnet run --project "04. CLICON/WS_Eurekabank_CLICON/WS_Eurekabank_CLICON"
# En el menú: opción 1 (Iniciar Sesión)
# Usuario: Monster
# Contraseña: Monster9

# CLIESC (escritorio WinForms)
dotnet run --project "04. CLIESC/CliEsc_Eurekabank_Rest/CliEsc_Eurekabank_Rest_G04_.sln"
# En el formulario de login
# Servidor (IP): localhost (o tu IP)
# Usuario: Monster
# Contraseña: Monster9

# CLIWEB (web, ASP.NET)
dotnet run --project "04. CLIWEB/ClienteWebEurekaBank.sln"
# Abre http://localhost:5000 (o el puerto configurado)
# Usuario: Monster
# Contraseña: Monster9

# CLIMOVIL (Android)
# Compilar desde Android Studio y ejecutar en emulador/dispositivo
# IP del servidor: 10.9.8.137 (o actualizar en la UI)
# Usuario: Monster
# Contraseña: Monster9
```

## Pruebas rápidas contra el Gateway

```powershell
# Login
curl.exe -X POST "http://localhost:5069/Eureka/validar-usuario?usuario=Monster&password=Monster9"

# Ver movimientos
curl.exe "http://localhost:5069/Eureka/movimientos/00100001"
```

---

**Nota:** Estas credenciales son solo para desarrollo/testing. En producción, usa un gestor de secretos (AWS Secrets Manager, Azure Key Vault, etc.).
