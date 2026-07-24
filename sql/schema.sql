-- Este script es de referencia. Lo normal es dejar que EF Core Migrations
-- cree la base de datos automáticamente (ver README, sección "Primeros pasos").
-- Si prefieres crearla manualmente, aquí está la tabla principal:

CREATE DATABASE CartaDeclaratoriaDB;
GO

USE CartaDeclaratoriaDB;
GO

CREATE TABLE CartasDeclaratorias (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FechaElaboracion DATE NOT NULL,

    BeneficiarioNombreCompleto NVARCHAR(300) NOT NULL,
    BeneficiarioFechaNacimiento DATE NULL,
    BeneficiarioPaisNacimiento NVARCHAR(150) NULL,
    BeneficiarioEntidadNacimiento NVARCHAR(150) NULL,
    BeneficiarioDomicilio NVARCHAR(500) NULL,
    BeneficiarioNumIdentificacion NVARCHAR(100) NULL,
    BeneficiarioTipoIdentificacion NVARCHAR(100) NULL,
    BeneficiarioTelefono NVARCHAR(50) NULL,
    BeneficiarioCurp NVARCHAR(18) NULL,
    BeneficiarioOcupacion NVARCHAR(200) NULL,
    BeneficiarioDescripcionOcupacion NVARCHAR(500) NULL,

    RemesaFolio NVARCHAR(100) NOT NULL,
    Monto DECIMAL(18,2) NOT NULL,
    CuentaNumero NVARCHAR(100) NULL,
    Banco NVARCHAR(200) NULL,
    GiradorNombre NVARCHAR(300) NULL,
    GiradorOcupacion NVARCHAR(300) NULL,
    GiradorLocalidadEstado NVARCHAR(200) NULL,
    RelacionConGirador NVARCHAR(300) NULL,
    OrigenDestinoRecursos NVARCHAR(1000) NULL,
    PropietarioReal NVARCHAR(300) NULL,
    PropietarioRealLocalidadEstado NVARCHAR(200) NULL,

    CapturadoPorUsuarioId NVARCHAR(450) NULL,
    FechaCaptura DATETIME2 NOT NULL DEFAULT GETDATE(),
    NombreFirma NVARCHAR(300) NULL
);
GO

CREATE INDEX IX_CartasDeclaratorias_Folio ON CartasDeclaratorias(RemesaFolio);
GO
