CREATE TABLE [Participante].[Par_Organigrama] (
    [IdOrganigrama]      INT           NOT NULL,
    [IdEmpresa]          INT           NOT NULL,
    [Descripcion]        VARCHAR (100) NULL,
    [IdOrganigramaPadre] INT           NULL,
    [Nivel]              TINYINT       NULL,
    [IdEmpleado]         INT           NULL,
    CONSTRAINT [PK_Organigrama] PRIMARY KEY CLUSTERED ([IdOrganigrama] ASC, [IdEmpresa] ASC) ON [Participante],
    CONSTRAINT [FK_Empresa_Organigrama] FOREIGN KEY ([IdEmpresa]) REFERENCES [Participante].[Par_Empresa] ([IdParticipante])
) ON [Participante];


GO
CREATE NONCLUSTERED INDEX [IDX_Organigrama_001]
    ON [Participante].[Par_Organigrama]([IdEmpresa] ASC)
    ON [Indices];

