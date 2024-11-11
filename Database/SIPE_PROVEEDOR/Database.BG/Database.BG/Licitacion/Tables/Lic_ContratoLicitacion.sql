﻿CREATE TABLE [Licitacion].[Lic_ContratoLicitacion] (
    [IdContrato]            INT            IDENTITY (1, 1) NOT NULL,
    [NumeroContrato]        VARCHAR (8)    NULL,
    [Anio]                  INT            NULL,
    [Mes]                   INT            NULL,
    [Dia]                   INT            NULL,
    [TipoContrato]          INT            NULL,
    [Secuencial]            INT            NULL,
    [IdAdquisicion]         BIGINT         NULL,
    [NombreLicitacion]      VARCHAR (40)   NULL,
    [CodTipoContrato]       VARCHAR (20)   NULL,
    [CodLineaNegocio]       VARCHAR (20)   NULL,
    [CodTipoServicio]       VARCHAR (20)   NULL,
    [CodPlazoSuscripcion]   VARCHAR (20)   NULL,
    [AdministradorContrato] VARCHAR (100)  NULL,
    [IdEstado]              INT            NULL,
    [RutaFisicaArchivo]     VARCHAR (2000) NULL,
    [NombreArchivo]         VARCHAR (25)   NULL,
    [FechaGeneracion]       DATETIME       NULL,
    [UsuarioGeneracion]     VARCHAR (20)   NULL,
    [AprobacionApoderado1]  BIT            DEFAULT ((0)) NULL,
    [AprobacionApoderado2]  BIT            DEFAULT ((0)) NULL,
    [FechaCreacion]         DATETIME       NOT NULL,
    [UsuarioCreacion]       VARCHAR (20)   NOT NULL,
    [FechaModificacion]     DATETIME       NULL,
    [UsuarioModificacion]   VARCHAR (20)   NULL,
    [CodFormaPago]          INT            DEFAULT ((0)) NULL,
    CONSTRAINT [PK_ContratoLicitacion] PRIMARY KEY CLUSTERED ([IdContrato] ASC)
);
