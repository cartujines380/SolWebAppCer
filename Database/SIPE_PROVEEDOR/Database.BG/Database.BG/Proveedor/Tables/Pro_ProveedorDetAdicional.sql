CREATE TABLE [Proveedor].[Pro_ProveedorDetAdicional] (
    [IdDetAd]      INT          IDENTITY (1, 1) NOT NULL,
    [CodProveedor] VARCHAR (25) NULL,
    [IdTipoDetAd]  VARCHAR (5)  NULL,
    [Valor]        VARCHAR (25) NULL,
    [Tiempo]       INT          NULL,
    PRIMARY KEY CLUSTERED ([IdDetAd] ASC)
);


GO
CREATE NONCLUSTERED INDEX [idx_cod_proveedor_01]
    ON [Proveedor].[Pro_ProveedorDetAdicional]([CodProveedor] ASC);

