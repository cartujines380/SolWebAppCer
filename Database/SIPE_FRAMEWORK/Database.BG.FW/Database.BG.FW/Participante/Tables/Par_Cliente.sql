CREATE TABLE [Participante].[Par_Cliente] (
    [IdParticipante]        INT          NOT NULL,
    [IdEmpresa]             INT          NOT NULL,
    [IdOficina]             INT          NOT NULL,
    [PorcentajeDescuento]   MONEY        NULL,
    [IdVendedor]            INT          NULL,
    [ContribuyenteEspecial] BIT          NULL,
    [IdCalificacion]        VARCHAR (10) NULL,
    [Iva]                   BIT          NULL,
    [Estado]                VARCHAR (10) NULL,
    [IdOficinaGestion]      INT          NULL,
    [GastoAnual]            MONEY        NULL,
    CONSTRAINT [PK_Cliente] PRIMARY KEY CLUSTERED ([IdParticipante] ASC, [IdEmpresa] ASC) ON [Participante],
    CONSTRAINT [FK_Empresa_Cliente] FOREIGN KEY ([IdEmpresa]) REFERENCES [Participante].[Par_Empresa] ([IdParticipante]),
    CONSTRAINT [FK_Participante_Cliente] FOREIGN KEY ([IdParticipante]) REFERENCES [Participante].[Par_Participante] ([IdParticipante])
) ON [Participante];


GO
CREATE NONCLUSTERED INDEX [IDX_Cliente_001]
    ON [Participante].[Par_Cliente]([IdEmpresa] ASC)
    ON [Indices];

