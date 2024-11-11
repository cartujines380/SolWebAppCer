CREATE TABLE [Seguridad].[Seg_AutorizacionUsuario] (
    [IdAutorizacion]       INT          NOT NULL,
    [IdUsuario]            VARCHAR (20) NOT NULL,
    [NumAutorizacion]      INT          NULL,
    [Valor]                VARCHAR (50) NULL,
    [FechaInicio]          DATETIME     NULL,
    [FechaFin]             DATETIME     NULL,
    [IdUsuarioAutorizador] VARCHAR (20) NULL,
    [UsrAlterno]           VARCHAR (20) NULL,
    [UsrJefe]              VARCHAR (20) NULL,
    CONSTRAINT [PK_AutorizacionUsuario] PRIMARY KEY CLUSTERED ([IdAutorizacion] ASC, [IdUsuario] ASC) ON [Seguridad],
    CONSTRAINT [FK_Autorizacion_AutorizacionUsuario] FOREIGN KEY ([IdAutorizacion]) REFERENCES [Seguridad].[Seg_Autorizacion] ([IdAutorizacion]),
    CONSTRAINT [FK_RegistroCliente_AutorizacionUsuario] FOREIGN KEY ([IdUsuario]) REFERENCES [Participante].[Par_RegistroCliente] ([IdUsuario])
) ON [Seguridad];


GO
CREATE NONCLUSTERED INDEX [IDX_AutorizacionUsuario_001]
    ON [Seguridad].[Seg_AutorizacionUsuario]([IdUsuario] ASC)
    ON [Indices];

