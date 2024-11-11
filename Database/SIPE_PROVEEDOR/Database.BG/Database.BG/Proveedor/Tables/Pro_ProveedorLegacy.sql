CREATE TABLE [Proveedor].[Pro_ProveedorLegacy] (
    [CodProveedor] VARCHAR (10) NOT NULL,
    [CodLegacy]    VARCHAR (10) NOT NULL,
    [Principal]    VARCHAR (1)  NOT NULL,
    CONSTRAINT [PK_Pro_ProveedorLegacy] PRIMARY KEY CLUSTERED ([CodProveedor] ASC, [CodLegacy] ASC) ON [Proveedor]
) ON [Proveedor];

