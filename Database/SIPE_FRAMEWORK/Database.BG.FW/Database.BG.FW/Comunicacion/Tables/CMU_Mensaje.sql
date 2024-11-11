CREATE TABLE [Comunicacion].[CMU_Mensaje] (
    [IdMensaje]       INT          IDENTITY (1, 1) NOT NULL,
    [IdUsuarioOrigen] VARCHAR (20) NOT NULL,
    [FechaEnvia]      DATETIME     NOT NULL,
    PRIMARY KEY CLUSTERED ([IdMensaje] ASC) ON [Comunicacion],
    CONSTRAINT [FK_Mensaje_RegistroCliente] FOREIGN KEY ([IdUsuarioOrigen]) REFERENCES [Participante].[Par_RegistroCliente] ([IdUsuario])
) ON [Comunicacion];


GO
CREATE NONCLUSTERED INDEX [IDX_Mensaje_001]
    ON [Comunicacion].[CMU_Mensaje]([IdUsuarioOrigen] ASC)
    ON [Indices];

