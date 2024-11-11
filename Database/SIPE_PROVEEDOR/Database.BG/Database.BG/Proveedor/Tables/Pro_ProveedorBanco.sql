CREATE TABLE [Proveedor].[Pro_ProveedorBanco] (
    [CodProveedor]  VARCHAR (10) NOT NULL,
    [CodBanco]      VARCHAR (15) NOT NULL,
    [CuentaNum]     VARCHAR (18) NOT NULL,
    [CuentaTitular] VARCHAR (60) NULL,
    CONSTRAINT [PK_Pro_ProveedorBanco] PRIMARY KEY CLUSTERED ([CodProveedor] ASC, [CodBanco] ASC, [CuentaNum] ASC) ON [Proveedor],
    CONSTRAINT [FK_PBanco_Prov] FOREIGN KEY ([CodProveedor]) REFERENCES [Proveedor].[Pro_Proveedor] ([CodProveedor])
) ON [Proveedor];

