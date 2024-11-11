CREATE TABLE [Pedidos].[Ped_Almacen] (
    [IdEmpresa]         INT          NOT NULL,
    [CodAlmacen]        VARCHAR (4)  NOT NULL,
    [CodLegacy]         VARCHAR (3)  NOT NULL,
    [NomAlmacen]        VARCHAR (30) NOT NULL,
    [CodCiudad]         VARCHAR (2)  NOT NULL,
    [CodAlmacenSRI]     VARCHAR (5)  NULL,
    [CodLegacyOriginal] VARCHAR (3)  NOT NULL,
    CONSTRAINT [PK_Ped_Almacen] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [CodAlmacen] ASC, [CodLegacy] ASC) ON [Pedidos],
    CONSTRAINT [FK_Pro_Empresa_Ped_Almacen] FOREIGN KEY ([IdEmpresa]) REFERENCES [Proveedor].[Pro_Empresa] ([IdEmpresa])
) ON [Pedidos];

