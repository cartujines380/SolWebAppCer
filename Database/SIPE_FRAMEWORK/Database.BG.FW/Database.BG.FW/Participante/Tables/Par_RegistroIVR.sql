CREATE TABLE [Participante].[Par_RegistroIVR] (
    [IdParticipante] INT          NOT NULL,
    [Clave]          VARCHAR (50) NOT NULL,
    [FechaRegistro]  DATETIME     NULL,
    [Estado]         CHAR (1)     NULL,
    CONSTRAINT [PK_RegistroIVR] PRIMARY KEY CLUSTERED ([IdParticipante] ASC) ON [Participante],
    CONSTRAINT [FK_Participante_RegistroIVR] FOREIGN KEY ([IdParticipante]) REFERENCES [Participante].[Par_Participante] ([IdParticipante])
) ON [Participante];

