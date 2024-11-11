CREATE TABLE [Participante].[Par_ParametroParticipante] (
    [IdParticipante]  INT           NOT NULL,
    [IdParametro]     INT           NOT NULL,
    [IdTipoParametro] VARCHAR (10)  NULL,
    [Valor]           VARCHAR (100) NULL,
    CONSTRAINT [PK_ParametroParticipante_PK] PRIMARY KEY CLUSTERED ([IdParticipante] ASC, [IdParametro] ASC) ON [Participante],
    CONSTRAINT [FK_Participante_ParametroParticipante_FK1] FOREIGN KEY ([IdParticipante]) REFERENCES [Participante].[Par_Participante] ([IdParticipante])
) ON [Participante];

