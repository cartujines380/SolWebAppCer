CREATE TABLE [Participante].[Par_Contacto] (
    [IdParticipante] INT          NOT NULL,
    [IdPartContacto] INT          NOT NULL,
    [IdTipoContacto] VARCHAR (10) NOT NULL,
    CONSTRAINT [PK_Contacto] PRIMARY KEY CLUSTERED ([IdParticipante] ASC, [IdPartContacto] ASC) ON [Participante],
    CONSTRAINT [FK_Participante_Contacto] FOREIGN KEY ([IdParticipante]) REFERENCES [Participante].[Par_Participante] ([IdParticipante])
) ON [Participante];

