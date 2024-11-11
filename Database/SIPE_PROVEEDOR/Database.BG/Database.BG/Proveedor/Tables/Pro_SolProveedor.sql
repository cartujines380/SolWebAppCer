﻿CREATE TABLE [Proveedor].[Pro_SolProveedor] (
    [IdEmpresa]                INT           NULL,
    [IdSolicitud]              BIGINT        IDENTITY (1, 1) NOT NULL,
    [TipoSolicitud]            VARCHAR (10)  NULL,
    [TipoProveedor]            VARCHAR (10)  NULL,
    [CodSapProveedor]          VARCHAR (10)  NULL,
    [TipoIdentificacion]       VARCHAR (10)  NULL,
    [Identificacion]           VARCHAR (13)  NULL,
    [NomComercial]             VARCHAR (35)  NULL,
    [RazonSocial]              VARCHAR (100) NULL,
    [FechaSRI]                 DATETIME      NULL,
    [SectorComercial]          VARCHAR (3)   NULL,
    [Idioma]                   VARCHAR (2)   NULL,
    [CodGrupoProveedor]        VARCHAR (15)  NULL,
    [ClaseContribuyente]       VARCHAR (10)  NULL,
    [GenDocElec]               VARCHAR (2)   NULL,
    [FechaSolicitud]           DATETIME      NULL,
    [Estado]                   VARCHAR (10)  NULL,
    [GrupoTesoreria]           VARCHAR (10)  NULL,
    [CuentaAsociada]           VARCHAR (20)  NULL,
    [Autorizacion]             VARCHAR (10)  NULL,
    [TransfArticuProvAnterior] VARCHAR (13)  NULL,
    [DepSolicitando]           VARCHAR (100) NULL,
    [Responsable]              VARCHAR (100) NULL,
    [Aprobacion]               VARCHAR (100) NULL,
    [Comentario]               VARCHAR (500) NULL,
    [AnioConsti]               VARCHAR (4)   NULL,
    [LineaNegocio]             VARCHAR (10)  NULL,
    [princliente]              VARCHAR (500) NULL,
    [totalventas]              VARCHAR (10)  NULL,
    [PlazoEntrega]             BIGINT        NULL,
    [DespachaProvincia]        VARCHAR (10)  NULL,
    [GrupoCuenta]              VARCHAR (10)  NULL,
    [RetencionIva]             VARCHAR (10)  NULL,
    [RetencionIva2]            VARCHAR (10)  NULL,
    [RetencionFuente]          VARCHAR (10)  NULL,
    [RetencionFuente2]         VARCHAR (10)  NULL,
    [CondicionPago]            VARCHAR (10)  NULL,
    [GrupoCompra]              VARCHAR (3)   NULL,
    [GrupoEsquema]             VARCHAR (2)   NULL,
    [Ramo]                     VARCHAR (4)   NULL,
    CONSTRAINT [PK_PRO_SOLPROVEEDOR] PRIMARY KEY CLUSTERED ([IdSolicitud] ASC)
);
