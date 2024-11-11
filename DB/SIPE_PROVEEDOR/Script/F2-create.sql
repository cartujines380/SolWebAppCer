USE SIPE_PROVEEDOR
GO

--CREACION DE TABLAS

if exists (select top 1 1 from sysobjects where name = 'Lic_ContratoLicitacion' and type='U')
    DROP TABLE [Licitacion].[Lic_ContratoLicitacion]
GO

CREATE TABLE [Licitacion].[Lic_ContratoLicitacion] (
    IdContrato				INT IDENTITY(1,1) NOT NULL,
    NumeroContrato			VARCHAR(8) NULL,
    Anio					INT NULL,
	Mes						INT NULL,
	Dia						INT NULL,
	TipoContrato			INT NULL,
	Secuencial				INT NULL,
	IdAdquisicion			BIGINT NULL,
	NombreLicitacion		VARCHAR(40) NULL,
	CodTipoContrato			VARCHAR(20) NULL,
	CodLineaNegocio			VARCHAR(20) NULL,
	CodTipoServicio			VARCHAR(20) NULL,
	CodPlazoSuscripcion		VARCHAR(20) NULL,
	AdministradorContrato	VARCHAR(100) NULL,
	IdEstado				INT NULL,
	RutaFisicaArchivo		VARCHAR(2000) NULL,
	NombreArchivo			VARCHAR(25) NULL,
	FechaGeneracion			DATETIME NULL,
	UsuarioGeneracion		VARCHAR (20) NULL,
	AprobacionApoderado1	BIT DEFAULT(0),
	AprobacionApoderado2	BIT DEFAULT(0),
    FechaCreacion			DATETIME NOT NULL,
    UsuarioCreacion			VARCHAR(20) NOT NULL,
    FechaModificacion		DATETIME NULL,
    UsuarioModificacion		VARCHAR(20) NULL,
    CONSTRAINT [PK_ContratoLicitacion] PRIMARY KEY CLUSTERED (IdContrato ASC)
);

go

if exists (select top 1 1 from sysobjects where name = 'Lic_LogFlujoContrato' and type='U')
    DROP TABLE [Licitacion].[Lic_LogFlujoContrato]
GO

CREATE TABLE [Licitacion].[Lic_LogFlujoContrato] (
    IdLogFlujo				INT IDENTITY(1,1) NOT NULL,
    IdAccion				INT NOT NULL,
    IdContrato				INT NOT NULL,
	IdEstadoContrato		INT NOT NULL,
    UsuarioRegistro			VARCHAR(20) NOT NULL,
    FechaRegistro			DATETIME NOT NULL,
    CONSTRAINT [PK_LogFlujoContrato] PRIMARY KEY CLUSTERED (IdLogFlujo ASC)
);

go

if exists (select top 1 1 from sysobjects where name = 'Lic_Accion' and type='U')
    DROP TABLE [Licitacion].[Lic_Accion]
GO

CREATE TABLE [Licitacion].[Lic_Accion] (
    IdAccion				INT NOT NULL,
    Nombre					VARCHAR(50) NOT NULL,
    CONSTRAINT [PK_Accion] PRIMARY KEY CLUSTERED (IdAccion ASC)
);

go


if exists (select top 1 1 from sysobjects where name = 'Lic_EstadosContrato' and type='U')
    DROP TABLE [Licitacion].[Lic_EstadosContrato]
GO

CREATE TABLE [Licitacion].[Lic_EstadosContrato] (
    IdEstado				INT NOT NULL,
    Nombre					VARCHAR(50) NOT NULL,
    CONSTRAINT [PK_EstadosContrato] PRIMARY KEY CLUSTERED (IdEstado ASC)
);

go


if exists (select top 1 1 from sysobjects where name = 'Lic_RolesWorkflow' and type='U')
    DROP TABLE [Licitacion].[Lic_RolesWorkflow]
GO

CREATE TABLE [Licitacion].[Lic_RolesWorkflow] (
    IdRol				INT NOT NULL,
    Nombre				VARCHAR(50) NOT NULL,
    CONSTRAINT [PK_RolesWorkflow] PRIMARY KEY CLUSTERED (IdRol ASC)
);

go
