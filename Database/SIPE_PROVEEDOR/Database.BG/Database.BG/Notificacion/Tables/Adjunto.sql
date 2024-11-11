CREATE TABLE [Notificacion].[Adjunto] (
    [Id_Notificacion] INT           NOT NULL,
    [NomArchivo]      VARCHAR (100) NOT NULL,
    [Comunicado]      VARCHAR (1)   NULL,
    CONSTRAINT [PK_Adjunto_1] PRIMARY KEY CLUSTERED ([Id_Notificacion] ASC, [NomArchivo] ASC),
    CONSTRAINT [fk_AdjNotificacion] FOREIGN KEY ([Id_Notificacion]) REFERENCES [Notificacion].[Notificacion] ([Codigo])
);

