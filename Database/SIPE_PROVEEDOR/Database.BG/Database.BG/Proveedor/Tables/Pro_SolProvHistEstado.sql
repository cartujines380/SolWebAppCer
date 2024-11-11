CREATE TABLE [Proveedor].[Pro_SolProvHistEstado] (
    [IdObservacion]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [IdSolicitud]     BIGINT        NOT NULL,
    [Motivo]          VARCHAR (10)  NULL,
    [Observacion]     VARCHAR (500) NULL,
    [Usuario]         VARCHAR (100) NULL,
    [Fecha]           DATETIME      NULL,
    [EstadoSolicitud] VARCHAR (10)  NULL,
    CONSTRAINT [PK_PRO_SOLPROVHISTESTADO] PRIMARY KEY CLUSTERED ([IdObservacion] ASC),
    CONSTRAINT [FK_PRO_SOLP_REFERENCE_PRO_SOLP] FOREIGN KEY ([IdSolicitud]) REFERENCES [Proveedor].[Pro_SolProveedor] ([IdSolicitud])
);

