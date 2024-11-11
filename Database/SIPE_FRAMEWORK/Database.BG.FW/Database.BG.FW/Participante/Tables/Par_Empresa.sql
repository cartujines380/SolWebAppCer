CREATE TABLE [Participante].[Par_Empresa] (
    [IdParticipante]     INT           NOT NULL,
    [Nombre]             VARCHAR (100) NOT NULL,
    [IdCategoriaEmpresa] INT           NOT NULL,
    [Nivel]              TINYINT       NOT NULL,
    [IdEmpresaPadre]     INT           NOT NULL,
    [Licencia]           VARCHAR (200) NULL,
    [Marca]              VARCHAR (100) NULL,
    [NumeroPatronal]     VARCHAR (100) NULL,
    [IdZona]             VARCHAR (10)  NULL,
    [IdRazonSocial]      VARCHAR (10)  NULL,
    [FechaConstitucion]  DATETIME      NULL,
    CONSTRAINT [PK_Empresa] PRIMARY KEY CLUSTERED ([IdParticipante] ASC) ON [Participante],
    CONSTRAINT [FK_CategoriaEmpresa_Empresa] FOREIGN KEY ([IdCategoriaEmpresa]) REFERENCES [Participante].[Par_CategoriaEmpresa] ([IdCategoriaEmpresa]),
    CONSTRAINT [FK_Participante_Empresa] FOREIGN KEY ([IdParticipante]) REFERENCES [Participante].[Par_Participante] ([IdParticipante])
) ON [Participante];


GO
CREATE NONCLUSTERED INDEX [IDX_Empresa_001]
    ON [Participante].[Par_Empresa]([IdCategoriaEmpresa] ASC)
    ON [Indices];

