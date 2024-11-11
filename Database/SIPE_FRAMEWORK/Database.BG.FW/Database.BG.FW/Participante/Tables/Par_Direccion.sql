CREATE TABLE [Participante].[Par_Direccion] (
    [IdParticipante]   INT           NOT NULL,
    [IdDireccion]      INT           NOT NULL,
    [IdTipoDireccion]  VARCHAR (10)  NULL,
    [Direccion]        VARCHAR (100) NULL,
    [NumCasa]          INT           NULL,
    [CallePrincipal]   VARCHAR (100) NULL,
    [CalleTransversal] VARCHAR (100) NULL,
    [IdPais]           VARCHAR (10)  NULL,
    [IdProvincia]      VARCHAR (10)  NULL,
    [IdCiudad]         VARCHAR (10)  NULL,
    [IdParroquia]      VARCHAR (10)  NULL,
    [IdBarrio]         VARCHAR (10)  NULL,
    [HorarioContacto]  VARCHAR (200) NULL,
    [NombreContacto]   VARCHAR (100) NULL,
    CONSTRAINT [PK_Direccion] PRIMARY KEY CLUSTERED ([IdParticipante] ASC, [IdDireccion] ASC) ON [Participante],
    CONSTRAINT [FK_Participante_Direccion] FOREIGN KEY ([IdParticipante]) REFERENCES [Participante].[Par_Participante] ([IdParticipante])
) ON [Participante];

