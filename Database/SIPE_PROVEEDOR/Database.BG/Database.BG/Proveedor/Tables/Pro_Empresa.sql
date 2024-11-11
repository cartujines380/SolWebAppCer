CREATE TABLE [Proveedor].[Pro_Empresa] (
    [IdEmpresa]  INT           NOT NULL,
    [NomEmpresa] VARCHAR (100) NOT NULL,
    [Ruc]        VARCHAR (13)  NOT NULL,
    CONSTRAINT [PK_Pro_Empresa] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC) ON [Proveedor]
) ON [Proveedor];

