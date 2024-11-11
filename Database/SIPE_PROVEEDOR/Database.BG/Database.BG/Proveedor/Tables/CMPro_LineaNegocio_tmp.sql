CREATE TABLE [Proveedor].[CMPro_LineaNegocio_tmp] (
    [CodProveedor] VARCHAR (25)  NULL,
    [LineaNegocio] VARCHAR (100) NULL,
    [Principal]    BIT           DEFAULT ((1)) NULL
);


GO
CREATE NONCLUSTERED INDEX [Idx_CodProveedor_01]
    ON [Proveedor].[CMPro_LineaNegocio_tmp]([CodProveedor] ASC);

