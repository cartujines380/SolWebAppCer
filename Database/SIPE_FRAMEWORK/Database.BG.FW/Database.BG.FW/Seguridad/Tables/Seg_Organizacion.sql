CREATE TABLE [Seguridad].[Seg_Organizacion] (
    [IdOrganizacion]   INT           NOT NULL,
    [IdCategoria]      INT           NOT NULL,
    [IdAplicacion]     INT           NULL,
    [Descripcion]      VARCHAR (200) NOT NULL,
    [IdOrgPadre]       INT           NULL,
    [Nivel]            INT           NULL,
    [CodRefAplicativo] VARCHAR (8)   NULL,
    CONSTRAINT [PK_Organizacion] PRIMARY KEY CLUSTERED ([IdOrganizacion] ASC) ON [Seguridad],
    CONSTRAINT [FK_Aplicacion_Organizacion] FOREIGN KEY ([IdAplicacion]) REFERENCES [Seguridad].[Seg_Aplicacion] ([IdAplicacion]),
    CONSTRAINT [FK_Categoria_Organizacion] FOREIGN KEY ([IdCategoria]) REFERENCES [Seguridad].[Seg_Categoria] ([IdCategoria])
) ON [Seguridad];


GO
CREATE NONCLUSTERED INDEX [IDX_Organizacion_001]
    ON [Seguridad].[Seg_Organizacion]([IdAplicacion] ASC)
    ON [Indices];


GO
CREATE NONCLUSTERED INDEX [IDX_Organizacion_002]
    ON [Seguridad].[Seg_Organizacion]([IdCategoria] ASC)
    ON [Indices];

