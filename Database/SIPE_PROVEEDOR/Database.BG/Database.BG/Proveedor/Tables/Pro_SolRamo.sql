CREATE TABLE [Proveedor].[Pro_SolRamo] (
    [IdSolicitud] BIGINT       NOT NULL,
    [CodRAmo]     VARCHAR (10) NOT NULL,
    [Estado]      BIT          NULL,
    [IdRamo]      BIGINT       IDENTITY (1, 1) NOT NULL,
    [Principal]   BIT          NULL,
    CONSTRAINT [PK_Pro_SolRamo] PRIMARY KEY CLUSTERED ([IdRamo] ASC),
    CONSTRAINT [FK_PRO_SOLR_FK_SOLRAM_PRO_SOLP] FOREIGN KEY ([IdSolicitud]) REFERENCES [Proveedor].[Pro_SolProveedor] ([IdSolicitud])
);

