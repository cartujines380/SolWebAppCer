CREATE TABLE [Seguridad].[Seg_Categoria] (
    [IdCategoria] INT           NOT NULL,
    [Descripcion] VARCHAR (100) NOT NULL,
    [IdCatPadre]  INT           NULL,
    [Nivel]       INT           NULL,
    CONSTRAINT [PK_Categoria] PRIMARY KEY CLUSTERED ([IdCategoria] ASC) ON [Seguridad]
) ON [Seguridad];

