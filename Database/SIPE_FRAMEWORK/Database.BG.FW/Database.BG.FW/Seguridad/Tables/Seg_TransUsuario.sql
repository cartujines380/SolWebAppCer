CREATE TABLE [Seguridad].[Seg_TransUsuario] (
    [IdOrganizacion]     INT           NOT NULL,
    [IdTransaccion]      INT           NOT NULL,
    [IdOpcion]           INT           NOT NULL,
    [IdUsuario]          VARCHAR (20)  NOT NULL,
    [FechaInicial]       DATETIME      NULL,
    [FechaFinal]         DATETIME      NULL,
    [Estado]             CHAR (1)      NULL,
    [IdHorario]          INT           NULL,
    [TipoIdentificacion] CHAR (1)      NULL,
    [IdIdentificacion]   VARCHAR (100) NULL,
    [UsrReemplaza]       VARCHAR (20)  NULL,
    CONSTRAINT [PK_TransUsuario] PRIMARY KEY CLUSTERED ([IdOrganizacion] ASC, [IdTransaccion] ASC, [IdOpcion] ASC, [IdUsuario] ASC) ON [Seguridad],
    CONSTRAINT [FK_Horario_TransUsuario] FOREIGN KEY ([IdHorario]) REFERENCES [Seguridad].[Seg_Horario] ([IdHorario]),
    CONSTRAINT [FK_OpcionTrans_TransUsuario] FOREIGN KEY ([IdOrganizacion], [IdTransaccion], [IdOpcion]) REFERENCES [Seguridad].[Seg_OpcionTrans] ([IdOrganizacion], [IdTransaccion], [IdOpcion]),
    CONSTRAINT [FK_RegistroCliente_TransUsuario] FOREIGN KEY ([IdUsuario]) REFERENCES [Participante].[Par_RegistroCliente] ([IdUsuario])
) ON [Seguridad];


GO
CREATE NONCLUSTERED INDEX [IDX_TransUsuario_001]
    ON [Seguridad].[Seg_TransUsuario]([IdHorario] ASC)
    ON [Indices];


GO
CREATE NONCLUSTERED INDEX [IDX_TransUsuario_002]
    ON [Seguridad].[Seg_TransUsuario]([IdUsuario] ASC)
    ON [Indices];

