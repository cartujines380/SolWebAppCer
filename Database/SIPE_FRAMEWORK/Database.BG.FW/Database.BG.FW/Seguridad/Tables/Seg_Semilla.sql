CREATE TABLE [Seguridad].[Seg_Semilla] (
    [IdParticipante] INT           NOT NULL,
    [FechaAct]       DATETIME      NOT NULL,
    [Semilla]        VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Semilla] PRIMARY KEY CLUSTERED ([IdParticipante] ASC, [FechaAct] ASC) ON [Seguridad],
    CONSTRAINT [FK_Participante_Semilla] FOREIGN KEY ([IdParticipante]) REFERENCES [Participante].[Par_Participante] ([IdParticipante])
) ON [Seguridad];

