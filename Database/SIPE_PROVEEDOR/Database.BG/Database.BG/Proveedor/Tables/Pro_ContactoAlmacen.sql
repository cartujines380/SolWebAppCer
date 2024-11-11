CREATE TABLE [Proveedor].[Pro_ContactoAlmacen] (
    [IdContAlm]     INT          IDENTITY (1, 1) NOT NULL,
    [IdContacto]    INT          NULL,
    [CodAlmacen]    VARCHAR (3)  NULL,
    [CodPais]       VARCHAR (3)  NULL,
    [CodCiudad]     VARCHAR (20) NULL,
    [CodRegion]     VARCHAR (3)  NULL,
    [Estado]        CHAR (1)     NULL,
    [FechaRegistro] DATETIME     NULL,
    PRIMARY KEY CLUSTERED ([IdContAlm] ASC),
    FOREIGN KEY ([IdContacto]) REFERENCES [Proveedor].[Pro_ContactoProveedor] ([IdContacto]),
    FOREIGN KEY ([IdContacto]) REFERENCES [Proveedor].[Pro_ContactoProveedor] ([IdContacto]),
    FOREIGN KEY ([IdContacto]) REFERENCES [Proveedor].[Pro_ContactoProveedor] ([IdContacto]),
    FOREIGN KEY ([IdContacto]) REFERENCES [Proveedor].[Pro_ContactoProveedor] ([IdContacto]),
    FOREIGN KEY ([IdContacto]) REFERENCES [Proveedor].[Pro_ContactoProveedor] ([IdContacto]),
    FOREIGN KEY ([IdContacto]) REFERENCES [Proveedor].[Pro_ContactoProveedor] ([IdContacto])
);


GO
CREATE NONCLUSTERED INDEX [ix_Pro_ContactoAlmacen_02]
    ON [Proveedor].[Pro_ContactoAlmacen]([FechaRegistro] ASC);

