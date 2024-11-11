CREATE TABLE [Seguridad].[Seg_AprobacionNivel] (
    [IdEmpresa] INT          NOT NULL,
    [Modulo]    VARCHAR (10) NOT NULL,
    [Nivel]     VARCHAR (10) NOT NULL,
    [Ruc]       VARCHAR (13) NOT NULL,
    [Usuario]   VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_Seg_AprobacionNivel] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [Modulo] ASC, [Nivel] ASC, [Ruc] ASC, [Usuario] ASC) ON [Proveedor]
) ON [Proveedor];

