CREATE TABLE [Proveedor].[Pro_SolDocAdjunto] (
    [IdSolicitud]     BIGINT        NOT NULL,
    [IdSolDocAdjunto] BIGINT        IDENTITY (1, 1) NOT NULL,
    [CodDocumento]    VARCHAR (10)  NULL,
    [NomArchivo]      VARCHAR (255) NULL,
    [Archivo]         VARCHAR (255) NULL,
    [FechaCarga]      DATETIME      NULL,
    [Estado]          BIT           NULL,
    CONSTRAINT [PK_Pro_SolDocAdjunto] PRIMARY KEY CLUSTERED ([IdSolDocAdjunto] ASC),
    CONSTRAINT [FK_PRO_SOLDOC_REFERENCE_PRO_SOLP] FOREIGN KEY ([IdSolicitud]) REFERENCES [Proveedor].[Pro_SolProveedor] ([IdSolicitud])
);

