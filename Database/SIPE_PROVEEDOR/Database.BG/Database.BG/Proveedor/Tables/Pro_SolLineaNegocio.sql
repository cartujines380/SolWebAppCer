CREATE TABLE [Proveedor].[Pro_SolLineaNegocio] (
    [IdSolicitud]    BIGINT       NOT NULL,
    [CodigoSociedad] VARCHAR (10) NULL,
    [CodigoSeccion]  VARCHAR (3)  NULL,
    [IdLIneNegocio]  BIGINT       IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_PRO_SOLLINEANEGOCIO] PRIMARY KEY CLUSTERED ([IdLIneNegocio] ASC),
    CONSTRAINT [FK_PRO_SOLL_REFERENCE_PRO_SOLP] FOREIGN KEY ([IdSolicitud]) REFERENCES [Proveedor].[Pro_SolProveedor] ([IdSolicitud])
);

