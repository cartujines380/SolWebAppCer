CREATE TABLE [Seguridad].[Seg_Rol] (
    [IdRol]       INT           NOT NULL,
    [IdEmpresa]   INT           NOT NULL,
    [IdSucursal]  INT           NULL,
    [Descripcion] VARCHAR (260) NULL,
    [Status]      VARCHAR (10)  NULL,
    [Nombre]      VARCHAR (260) NOT NULL,
    [Abreviatura] VARCHAR (10)  NULL,
    CONSTRAINT [PK_Rol] PRIMARY KEY CLUSTERED ([IdRol] ASC) ON [Seguridad],
    CONSTRAINT [FK_Empresa_Rol] FOREIGN KEY ([IdEmpresa]) REFERENCES [Participante].[Par_Empresa] ([IdParticipante])
) ON [Seguridad];


GO
CREATE NONCLUSTERED INDEX [IDX_Rol_001]
    ON [Seguridad].[Seg_Rol]([IdEmpresa] ASC)
    ON [Indices];

