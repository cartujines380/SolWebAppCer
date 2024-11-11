CREATE TABLE [Participante].[Par_ListaDistribucion] (
    [IdParticipante] INT           NOT NULL,
    [IdLista]        INT           NOT NULL,
    [Descripcion]    VARCHAR (100) NULL,
    CONSTRAINT [PK_ListaDistribucion] PRIMARY KEY CLUSTERED ([IdParticipante] ASC, [IdLista] ASC) ON [Participante],
    CONSTRAINT [FK_Participante_ListaDistribucion] FOREIGN KEY ([IdParticipante]) REFERENCES [Participante].[Par_Participante] ([IdParticipante])
) ON [Participante];

