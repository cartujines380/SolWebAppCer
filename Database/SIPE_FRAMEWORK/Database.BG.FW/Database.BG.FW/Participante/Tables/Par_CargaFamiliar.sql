CREATE TABLE [Participante].[Par_CargaFamiliar] (
    [IdParticipante]       INT           NOT NULL,
    [IdCargaFamiliar]      INT           NOT NULL,
    [IdTipoCarga]          VARCHAR (10)  NULL,
    [IdTipoIdentificacion] VARCHAR (10)  NULL,
    [Identificacion]       VARCHAR (13)  NULL,
    [Nombre]               VARCHAR (100) NULL,
    [FechaNac]             DATETIME      NULL,
    CONSTRAINT [PK_CargaFamiliar] PRIMARY KEY CLUSTERED ([IdParticipante] ASC, [IdCargaFamiliar] ASC) ON [Participante],
    CONSTRAINT [FK_Persona_CargaFamiliar] FOREIGN KEY ([IdParticipante]) REFERENCES [Participante].[Par_Persona] ([IdParticipante])
) ON [Participante];

