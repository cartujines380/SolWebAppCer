CREATE TABLE [Seguridad].[Seg_UsuarioZona] (
    [IdEmpresa] INT          NOT NULL,
    [Ruc]       VARCHAR (13) NOT NULL,
    [Usuario]   VARCHAR (20) NOT NULL,
    [Zona]      VARCHAR (12) NOT NULL,
    CONSTRAINT [PK_Seg_UsuarioZona] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [Ruc] ASC, [Usuario] ASC, [Zona] ASC) ON [Proveedor]
) ON [Proveedor];

