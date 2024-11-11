CREATE TABLE [Seguridad].[Seg_RolUsuario] (
    [IdRol]              INT           NOT NULL,
    [IdUsuario]          VARCHAR (20)  NOT NULL,
    [IdHorario]          INT           NULL,
    [Estado]             VARCHAR (10)  NULL,
    [FechaInicial]       DATETIME      NULL,
    [FechaFinal]         DATETIME      NULL,
    [TipoIdentificacion] CHAR (1)      NULL,
    [IdIdentificacion]   VARCHAR (100) NULL,
    [UsrReemplazo]       VARCHAR (20)  NULL,
    CONSTRAINT [PK_RolUsuario] PRIMARY KEY CLUSTERED ([IdRol] ASC, [IdUsuario] ASC) ON [Seguridad],
    CONSTRAINT [FK_Horario_RolUsuario] FOREIGN KEY ([IdHorario]) REFERENCES [Seguridad].[Seg_Horario] ([IdHorario]),
    CONSTRAINT [FK_RegistroCliente_RolUsuario] FOREIGN KEY ([IdUsuario]) REFERENCES [Participante].[Par_RegistroCliente] ([IdUsuario]),
    CONSTRAINT [FK_Rol_RolUsuario] FOREIGN KEY ([IdRol]) REFERENCES [Seguridad].[Seg_Rol] ([IdRol])
) ON [Seguridad];


GO
CREATE NONCLUSTERED INDEX [IDX_RolUsuario_001]
    ON [Seguridad].[Seg_RolUsuario]([IdHorario] ASC)
    ON [Indices];


GO
CREATE NONCLUSTERED INDEX [IDX_RolUsuario_002]
    ON [Seguridad].[Seg_RolUsuario]([IdUsuario] ASC)
    ON [Indices];


GO
CREATE NONCLUSTERED INDEX [IDX_RolUsuario_003]
    ON [Seguridad].[Seg_RolUsuario]([FechaInicial] ASC, [FechaFinal] ASC)
    ON [Seguridad];

