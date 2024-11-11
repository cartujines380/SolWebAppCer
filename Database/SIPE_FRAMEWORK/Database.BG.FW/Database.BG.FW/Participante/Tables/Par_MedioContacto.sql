CREATE TABLE [Participante].[Par_MedioContacto] (
    [IdParticipante]      INT           NOT NULL,
    [IdDireccion]         INT           NOT NULL,
    [IdMedioContacto]     INT           NOT NULL,
    [IdTipoMedioContacto] VARCHAR (10)  NOT NULL,
    [Valor]               VARCHAR (100) NULL,
    [ValorAlt]            VARCHAR (20)  NULL,
    CONSTRAINT [PK_MedioContacto] PRIMARY KEY CLUSTERED ([IdParticipante] ASC, [IdDireccion] ASC, [IdTipoMedioContacto] ASC, [IdMedioContacto] ASC) ON [Participante],
    CONSTRAINT [FK_Direccion_MedioContacto] FOREIGN KEY ([IdParticipante], [IdDireccion]) REFERENCES [Participante].[Par_Direccion] ([IdParticipante], [IdDireccion])
) ON [Participante];

