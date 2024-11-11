CREATE TABLE [Proveedor].[Pro_ProveedorDetalle] (
    [IdDetalle]              INT            IDENTITY (1, 1) NOT NULL,
    [CodProveedor]           VARCHAR (25)   NULL,
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
    [Calificacion]           VARCHAR (5)    NULL,
    [FecTermCalificacion]    VARCHAR (20)   NULL,
    [UsuarioCreacion]        VARCHAR (15)   NULL,
    [FechaCreacion]          DATETIME       NULL,
    [CodActividadEconomica]  VARCHAR (20)   NULL,
    [IdProveedor]            VARCHAR (10)   NULL,
    [TipoCalificacion]       VARCHAR (5)    NULL,
    [TipoServicio]           VARCHAR (10)   NULL,
    [RelacionBanco]          BIT            NULL,
    [RelacionIdentificacion] VARCHAR (20)   NULL,
    [RelacionNombres]        VARCHAR (100)  NULL,
    [RelacionArea]           VARCHAR (50)   NULL,
    [PersonaExpuesta]        BIT            NULL,
    [EleccionPopular]        BIT            NULL,
    PRIMARY KEY CLUSTERED ([IdDetalle] ASC)
);


GO
CREATE NONCLUSTERED INDEX [idx_cod_proveedor_01]
    ON [Proveedor].[Pro_ProveedorDetalle]([CodProveedor] ASC);

