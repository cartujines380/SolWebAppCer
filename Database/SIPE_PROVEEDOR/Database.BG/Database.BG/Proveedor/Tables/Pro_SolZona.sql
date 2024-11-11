CREATE TABLE [Proveedor].[Pro_SolZona] (
    [IdSolicitud] BIGINT       NOT NULL,
    [CodZona]     VARCHAR (12) NOT NULL,
    [Idzona]      BIGINT       IDENTITY (1, 1) NOT NULL,
    [Estado]      BIT          NULL,
    CONSTRAINT [PK_PRO_SOLZONA] PRIMARY KEY CLUSTERED ([IdSolicitud] ASC, [CodZona] ASC),
    CONSTRAINT [FK_PRO_SOLZ_FK_SOLZON_PRO_SOLP] FOREIGN KEY ([IdSolicitud]) REFERENCES [Proveedor].[Pro_SolProveedor] ([IdSolicitud])
);

