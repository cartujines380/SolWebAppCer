CREATE TABLE [Participante].[Par_Oficina] (
    [IdEmpresa]  INT          NOT NULL,
    [IdOficina]  INT          NOT NULL,
    [IdZona]     VARCHAR (10) NULL,
    [OficinaSRI] INT          NULL,
    CONSTRAINT [PK_Oficina] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdOficina] ASC) ON [Participante],
    CONSTRAINT [FK_Empresa_Oficina] FOREIGN KEY ([IdEmpresa]) REFERENCES [Participante].[Par_Empresa] ([IdParticipante])
) ON [Participante];

