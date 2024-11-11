CREATE TABLE [Seguridad].[Seg_UsrZonaAlmacen] (
    [IdEmpresa] INT          NOT NULL,
    [Ruc]       VARCHAR (13) NOT NULL,
    [Usuario]   VARCHAR (20) NOT NULL,
    [Zona]      VARCHAR (12) NOT NULL,
    [Almacen]   VARCHAR (4)  NOT NULL,
    CONSTRAINT [PK_Seg_UsrZonaAlmacen] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [Ruc] ASC, [Usuario] ASC, [Zona] ASC, [Almacen] ASC) ON [Proveedor]
) ON [Proveedor];

