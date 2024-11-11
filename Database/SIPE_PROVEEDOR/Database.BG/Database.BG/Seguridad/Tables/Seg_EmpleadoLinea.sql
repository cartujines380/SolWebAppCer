CREATE TABLE [Seguridad].[Seg_EmpleadoLinea] (
    [IdEmpresa] INT          NOT NULL,
    [Ruc]       VARCHAR (13) NOT NULL,
    [Usuario]   VARCHAR (20) NOT NULL,
    [Linea]     VARCHAR (10) NOT NULL,
    CONSTRAINT [PK_Seg_EmpleadoLinea] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [Ruc] ASC, [Usuario] ASC, [Linea] ASC) ON [Proveedor]
) ON [Proveedor];

