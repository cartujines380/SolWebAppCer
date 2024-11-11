CREATE TABLE [Participante].[Par_Notificacion] (
    [IdParticipante] INT            NOT NULL,
    [FechaEnvio]     DATETIME       NOT NULL,
    [IdEmpresa]      INT            NOT NULL,
    [IdOficina]      INT            NOT NULL,
    [IdTipoEnvio]    INT            NULL,
    [Usuarios]       VARCHAR (1000) NULL,
    [UsuariosCopia]  VARCHAR (100)  NULL,
    [Mensaje]        VARCHAR (MAX)  NULL,
    CONSTRAINT [PK_Notificacion] PRIMARY KEY CLUSTERED ([IdParticipante] ASC, [FechaEnvio] ASC) ON [Participante],
    CONSTRAINT [FK_Participante_Notificacion] FOREIGN KEY ([IdParticipante]) REFERENCES [Participante].[Par_Participante] ([IdParticipante])
) ON [Participante] TEXTIMAGE_ON [Participante];

