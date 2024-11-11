CREATE TABLE [Catalogo].[Ctl_CatalogoEmpresa] (
    [IdEmpresa] INT          NOT NULL,
    [IdTabla]   INT          NOT NULL,
    [Codigo]    VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_CargaFamiliar] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdTabla] ASC, [Codigo] ASC) ON [Catalogo],
    CONSTRAINT [FK_Tabla_Empresa_Catalogo] FOREIGN KEY ([IdTabla], [Codigo]) REFERENCES [Catalogo].[Ctl_Catalogo] ([IdTabla], [Codigo])
) ON [Catalogo];


GO
CREATE NONCLUSTERED INDEX [IDX_CatalogoEmpresa_001]
    ON [Catalogo].[Ctl_CatalogoEmpresa]([IdTabla] ASC)
    ON [Indices];

