CREATE TABLE [Participante].[Par_CategoriaEmpresa] (
    [IdCategoriaEmpresa]  INT           NOT NULL,
    [Descripcion]         VARCHAR (100) NULL,
    [IdCategoriaEmpPadre] INT           NULL,
    [Nivel]               SMALLINT      NULL,
    CONSTRAINT [PK_CategoriaEmpresa] PRIMARY KEY CLUSTERED ([IdCategoriaEmpresa] ASC) ON [Participante]
) ON [Participante];

