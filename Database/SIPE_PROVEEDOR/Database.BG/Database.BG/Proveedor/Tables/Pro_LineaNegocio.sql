CREATE TABLE [Proveedor].[Pro_LineaNegocio] (
    [CodProveedor] VARCHAR (10) NOT NULL,
    [LineaNegocio] VARCHAR (10) NOT NULL,
    [Principal]    BIT          NOT NULL,
    CONSTRAINT [PK_Pro_LineaNegocio] PRIMARY KEY CLUSTERED ([CodProveedor] ASC, [LineaNegocio] ASC),
    CONSTRAINT [FK_LNegocio_Prov] FOREIGN KEY ([CodProveedor]) REFERENCES [Proveedor].[Pro_Proveedor] ([CodProveedor])
);

