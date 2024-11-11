CREATE TABLE [Proveedor].[Pro_SolProveedorDetalle] (
    [IdDetalle]              INT            IDENTITY (1, 1) NOT NULL,
    [IdSolicitud]            BIGINT         NOT NULL,
    [EsCritico]              CHAR (1)       NULL,
    [ProcesoBrindaSoporte]   VARCHAR (850)  NULL,
    [EsServiciosAux]         CHAR (1)       NULL,
    [IdArea]                 VARCHAR (5)    NULL,
    [IdFormasPago]           VARCHAR (5)    NULL,
    [IdStatus]               VARCHAR (5)    NULL,
    [LlenadoDocVinculacion]  CHAR (1)       NULL,
    [VinculadoBG]            CHAR (1)       NULL,
    [Recurrente]             CHAR (1)       NULL,
    [FirmadoSegInfo]         CHAR (1)       NULL,
    [FirmadoPCI]             CHAR (1)       NULL,
    [CalificadoContNegocio]  VARCHAR (12)   NULL,
    [Sgs]                    VARCHAR (1000) NULL,
    [TipoCalificacion]       VARCHAR (5)    NULL,
    [Calificacion]           VARCHAR (5)    NULL,
    [FecTermCalificacion]    VARCHAR (20)   NULL,
    [UsuarioCreacion]        VARCHAR (15)   NULL,
    [FechaCreacion]          DATETIME       NULL,
    [CodActividadEconomica]  VARCHAR (20)   NULL,
    [TipoServicio]           VARCHAR (10)   NULL,
    [RelacionBanco]          BIT            NULL,
    [RelacionIdentificacion] VARCHAR (20)   NULL,
    [RelacionNombres]        VARCHAR (100)  NULL,
    [RelacionArea]           VARCHAR (50)   NULL,
    [PersonaExpuesta]        BIT            NULL,
    [EleccionPopular]        BIT            NULL,
    PRIMARY KEY CLUSTERED ([IdDetalle] ASC),
    CONSTRAINT [FK_PRO_SOLD_REFERENCE_PRO_SOLP] FOREIGN KEY ([IdSolicitud]) REFERENCES [Proveedor].[Pro_SolProveedor] ([IdSolicitud])
);


GO
CREATE NONCLUSTERED INDEX [ix_Pro_SolProveedorDetalle_01]
    ON [Proveedor].[Pro_SolProveedorDetalle]([IdSolicitud] ASC);

