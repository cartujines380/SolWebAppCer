CREATE TABLE [Participante].[Par_Proveedor] (
    [IdParticipante]        INT          NOT NULL,
    [IdEmpresa]             INT          NOT NULL,
    [IdOficina]             INT          NOT NULL,
    [ContribuyenteEspecial] BIT          NULL,
    [Estado]                VARCHAR (10) NULL,
    [Acepta]                CHAR (1)     NULL,
    CONSTRAINT [PK_Proveedor] PRIMARY KEY CLUSTERED ([IdParticipante] ASC, [IdEmpresa] ASC) ON [Participante],
    CONSTRAINT [FK_Participante_Proveedor] FOREIGN KEY ([IdParticipante]) REFERENCES [Participante].[Par_Participante] ([IdParticipante])
) ON [Participante];

