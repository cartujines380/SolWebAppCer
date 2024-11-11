CREATE TABLE [Participante].[Par_LoteHac] (
    [IdEmpresa]     INT           NOT NULL,
    [IdLote]        INT           NOT NULL,
    [Tamaño]        FLOAT (53)    NULL,
    [IdTipoCultivo] VARCHAR (10)  NULL,
    [Copropietario] VARCHAR (200) NULL,
    CONSTRAINT [PK_LotesHac] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [IdLote] ASC) ON [Participante],
    CONSTRAINT [FK_Participante_LotesHac] FOREIGN KEY ([IdEmpresa]) REFERENCES [Participante].[Par_Empresa] ([IdParticipante])
) ON [Participante];

