CREATE TABLE [Participante].[Par_RegistroCliente] (
    [IdUsuario]         VARCHAR (20)  NOT NULL,
    [IdParticipante]    INT           NOT NULL,
    [RolAsignado]       VARCHAR (10)  NOT NULL,
    [IdUsuarioRegistro] VARCHAR (50)  NULL,
    [FechaRegistro]     DATETIME      NOT NULL,
    [OrigenRegistro]    CHAR (1)      NULL,
    [TipoTarjeta]       CHAR (1)      NULL,
    [NumeroTarjeta]     VARCHAR (16)  NULL,
    [TipoIdent]         CHAR (1)      NULL,
    [NumIdent]          VARCHAR (15)  NULL,
    [TipoPlanTrans]     TINYINT       NULL,
    [TipoCliente]       TINYINT       NULL,
    [Estado]            BIT           NOT NULL,
    [PregSecreta]       VARCHAR (100) NULL,
    [RespSecreta]       VARCHAR (100) NULL,
    [FraseSecreta]      VARCHAR (200) NULL,
    [AdmProducto]       BIT           NULL,
    [IdTipoLogin]       VARCHAR (10)  NULL,
    PRIMARY KEY CLUSTERED ([IdUsuario] ASC) ON [Participante],
    CONSTRAINT [FK_Participante_RegistroCliente] FOREIGN KEY ([IdParticipante]) REFERENCES [Participante].[Par_Participante] ([IdParticipante])
) ON [Participante];


GO
CREATE NONCLUSTERED INDEX [IDX_RegistroCliente_001]
    ON [Participante].[Par_RegistroCliente]([IdParticipante] ASC)
    ON [Indices];

