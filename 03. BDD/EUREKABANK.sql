/*
Empresa        :  EurekaBank
Software       :  Sistema de Cuentas de Ahorro (Versión REST)
DBMS           :  SQL Server
Base de Datos  :  eurekanetawsrest
Script         :  Crea y Carga la Base de Datos sin errores en AWS
*/

USE master;
GO

-- Crear la base de datos directamente para evitar errores de permisos en RDS
CREATE DATABASE [eurekanetawsrest];
GO

USE [eurekanetawsrest];
GO

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- ============================================================================
-- Tabla: auth
-- ============================================================================
CREATE TABLE dbo.auth(
    USUARIO   VARCHAR(50)  NOT NULL,
    PASSWORD  VARBINARY(32) NOT NULL,
    CONSTRAINT PK_auth PRIMARY KEY (USUARIO)
);
GO

CREATE INDEX IX_auth_usuario ON dbo.auth (USUARIO);
GO

-- ============================================================================
-- Tabla: empleado
-- ============================================================================
CREATE TABLE dbo.empleado(
    chr_emplcodigo  VARCHAR(20)  NOT NULL,
    vch_emplnombre  VARCHAR(100) NOT NULL,
    vch_emplestado  VARCHAR(20)  NOT NULL CONSTRAINT DF_empleado_estado DEFAULT ('ACTIVO'),
    CONSTRAINT PK_empleado PRIMARY KEY (chr_emplcodigo),
    CONSTRAINT CK_empleado_estado CHECK (vch_emplestado IN ('ACTIVO', 'INACTIVO'))
);
GO

-- ============================================================================
-- Tabla: tipomovimiento
-- ============================================================================
CREATE TABLE dbo.tipomovimiento(
    chr_tipocodigo      CHAR(3)      NOT NULL,
    vch_tipodescripcion VARCHAR(100) NOT NULL,
    vch_tipoaccion      VARCHAR(20)  NOT NULL,
    CONSTRAINT PK_tipomovimiento PRIMARY KEY (chr_tipocodigo),
    CONSTRAINT CK_tipoaccion CHECK (vch_tipoaccion IN ('ENTRADA', 'SALIDA'))
);
GO

-- ============================================================================
-- Tabla: cuenta
-- ============================================================================
CREATE TABLE dbo.cuenta(
    chr_cuencodigo    VARCHAR(20)   NOT NULL,
    dec_cuensaldo     DECIMAL(18,2) NOT NULL CONSTRAINT DF_cuenta_saldo DEFAULT (0),
    int_cuencontmov   INT           NOT NULL CONSTRAINT DF_cuenta_contmov DEFAULT (0),
    vch_cuenestado    VARCHAR(20)   NOT NULL CONSTRAINT DF_cuenta_estado DEFAULT ('ACTIVO'),
    CONSTRAINT PK_cuenta PRIMARY KEY (chr_cuencodigo),
    CONSTRAINT CK_cuenta_saldo CHECK (dec_cuensaldo >= 0),
    CONSTRAINT CK_cuenta_estado CHECK (vch_cuenestado IN ('ACTIVO', 'INACTIVO', 'CERRADO'))
);
GO

-- ============================================================================
-- Tabla: movimiento
-- ============================================================================
CREATE TABLE dbo.movimiento(
    chr_cuencodigo   VARCHAR(20)   NOT NULL,
    int_movinumero   INT           NOT NULL,
    dtt_movifecha    DATETIME2(0)  NOT NULL CONSTRAINT DF_movimiento_fecha DEFAULT (SYSDATETIME()),
    chr_emplcodigo   VARCHAR(20)   NOT NULL,
    chr_tipocodigo   CHAR(3)       NOT NULL,
    dec_moviimporte  DECIMAL(18,2) NOT NULL,
    CONSTRAINT PK_movimiento PRIMARY KEY (chr_cuencodigo, int_movinumero),
    CONSTRAINT FK_movimiento_cuenta FOREIGN KEY (chr_cuencodigo) REFERENCES dbo.cuenta(chr_cuencodigo),
    CONSTRAINT FK_movimiento_empleado FOREIGN KEY (chr_emplcodigo) REFERENCES dbo.empleado(chr_emplcodigo),
    CONSTRAINT FK_movimiento_tipomovimiento FOREIGN KEY (chr_tipocodigo) REFERENCES dbo.tipomovimiento(chr_tipocodigo),
    CONSTRAINT CK_movimiento_importe CHECK (dec_moviimporte > 0)
);
GO

-- Índices de optimización
CREATE INDEX IX_movimiento_cuenta_fecha ON dbo.movimiento (chr_cuencodigo, dtt_movifecha DESC);
GO
CREATE INDEX IX_movimiento_tipo ON dbo.movimiento (chr_tipocodigo);
GO
CREATE INDEX IX_movimiento_empleado ON dbo.movimiento (chr_emplcodigo);
GO

-- ============================================================================
-- INSERCIÓN DE DATOS MÍNIMOS
-- ============================================================================
INSERT INTO dbo.empleado (chr_emplcodigo, vch_emplnombre, vch_emplestado)
VALUES ('0001', 'Empleado Administrador', 'ACTIVO');
GO

INSERT INTO dbo.tipomovimiento (chr_tipocodigo, vch_tipodescripcion, vch_tipoaccion)
VALUES 
('003', 'Deposito', 'ENTRADA'),
('004', 'Retiro', 'SALIDA'),
('008', 'Transferencia recibida', 'ENTRADA'),
('009', 'Transferencia enviada', 'SALIDA');
GO

INSERT INTO dbo.cuenta (chr_cuencodigo, dec_cuensaldo, int_cuencontmov, vch_cuenestado)
VALUES
('00100001', 1000.00, 0, 'ACTIVO'),
('00100002', 500.00, 0, 'ACTIVO');
GO

INSERT INTO dbo.auth (USUARIO, PASSWORD)
VALUES ('Monster', HASHBYTES('SHA2_256', N'Monster9'));
GO

-- ============================================================================
-- VERIFICACIÓN Y REPORTE FINAL
-- ============================================================================
PRINT '===== CREACIÓN DE BASE DE DATOS EXITOSA =====';
PRINT '';
SELECT 'auth' AS Tabla, COUNT(*) AS Registros FROM dbo.auth
UNION ALL
SELECT 'empleado', COUNT(*) FROM dbo.empleado
UNION ALL
SELECT 'tipomovimiento', COUNT(*) FROM dbo.tipomovimiento
UNION ALL
SELECT 'cuenta', COUNT(*) FROM dbo.cuenta
UNION ALL
SELECT 'movimiento', COUNT(*) FROM dbo.movimiento;
GO