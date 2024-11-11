CREATE TABLE [Participante].[Par_Persona] (
    [IdParticipante]   INT           NOT NULL,
    [Apellido1]        VARCHAR (100) NULL,
    [Apellido2]        VARCHAR (100) NULL,
    [Nombre1]          VARCHAR (100) NULL,
    [Nombre2]          VARCHAR (100) NULL,
    [IdTitulo]         VARCHAR (10)  NULL,
    [Sexo]             VARCHAR (10)  NULL,
    [FechaNacimiento]  DATETIME      NULL,
    [EstadoCivil]      VARCHAR (10)  NULL,
    [Ruc]              VARCHAR (100) NULL,
    [IdEmpRel]         INT           NULL,
    [IdNivelEducacion] VARCHAR (10)  NULL,
    CONSTRAINT [PK_Persona] PRIMARY KEY CLUSTERED ([IdParticipante] ASC) ON [Participante],
    CONSTRAINT [FK_Participante_Persona] FOREIGN KEY ([IdParticipante]) REFERENCES [Participante].[Par_Participante] ([IdParticipante])
) ON [Participante];

