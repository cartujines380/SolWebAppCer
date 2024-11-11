CREATE TABLE [Seguridad].[Seg_Aplicacion] (
    [IdAplicacion] INT           NOT NULL,
    [IdEmpresa]    INT           NOT NULL,
    [Nombre]       VARCHAR (30)  NULL,
    [Descripcion]  VARCHAR (100) NOT NULL,
    [TipoServidor] VARCHAR (10)  NULL,
    [Datagrama]    VARCHAR (100) NULL,
    [Link]         VARCHAR (200) NULL,
    CONSTRAINT [PK_Aplicacion] PRIMARY KEY CLUSTERED ([IdAplicacion] ASC) ON [Seguridad],
    CONSTRAINT [FK_Empresa_Aplicacion] FOREIGN KEY ([IdEmpresa]) REFERENCES [Participante].[Par_Empresa] ([IdParticipante])
) ON [Seguridad];


GO
CREATE NONCLUSTERED INDEX [IDX_Aplicacion_001]
    ON [Seguridad].[Seg_Aplicacion]([IdEmpresa] ASC)
    ON [Indices];

