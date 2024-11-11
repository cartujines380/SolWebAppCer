CREATE TABLE [Proveedor].[Pro_SolViapago] (
    [IdSolicitud] BIGINT       NOT NULL,
    [CodVia]      VARCHAR (10) NOT NULL,
    [Estado]      BIT          NULL,
    [IdVia]       BIGINT       IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_Pro_SolVia] PRIMARY KEY CLUSTERED ([IdVia] ASC),
    CONSTRAINT [FK_PRO_SOLR_FK_SOLVIA_PRO_SOLP] FOREIGN KEY ([IdSolicitud]) REFERENCES [Proveedor].[Pro_SolProveedor] ([IdSolicitud])
);

