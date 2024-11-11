CREATE TABLE [Proveedor].[Pro_DocAdjunto] (
    [CodProveedor] VARCHAR (10)  NOT NULL,
    [IdDocAdjunto] BIGINT        IDENTITY (1, 1) NOT NULL,
    [CodDocumento] VARCHAR (10)  NULL,
    [NomArchivo]   VARCHAR (255) NULL,
    [Archivo]      VARCHAR (255) NULL,
    [FechaCarga]   DATETIME      NULL,
    [Estado]       BIT           NULL,
    PRIMARY KEY CLUSTERED ([IdDocAdjunto] ASC),
    CONSTRAINT [FK_PRO_DOC_REFERENCE_PRO_P] FOREIGN KEY ([CodProveedor]) REFERENCES [Proveedor].[Pro_Proveedor] ([CodProveedor])
);


GO
CREATE NONCLUSTERED INDEX [ix_Pro_IdDocAdjunto_01]
    ON [Proveedor].[Pro_DocAdjunto]([IdDocAdjunto] ASC);

