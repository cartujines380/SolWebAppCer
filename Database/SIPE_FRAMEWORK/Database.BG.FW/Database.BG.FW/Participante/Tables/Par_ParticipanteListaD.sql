CREATE TABLE [Participante].[Par_ParticipanteListaD] (
    [IdParticipante] INT NOT NULL,
    [IdLista]        INT NOT NULL,
    [IdPartLista]    INT NOT NULL,
    CONSTRAINT [PK_ParticipanteListaD] PRIMARY KEY CLUSTERED ([IdParticipante] ASC, [IdLista] ASC, [IdPartLista] ASC) ON [Participante],
    CONSTRAINT [FK_ListaDistribucion_ParticipanteListaD] FOREIGN KEY ([IdParticipante], [IdLista]) REFERENCES [Participante].[Par_ListaDistribucion] ([IdParticipante], [IdLista]),
    CONSTRAINT [FK_Participante_ParticipanteListaD] FOREIGN KEY ([IdPartLista]) REFERENCES [Participante].[Par_Participante] ([IdParticipante])
) ON [Participante];

