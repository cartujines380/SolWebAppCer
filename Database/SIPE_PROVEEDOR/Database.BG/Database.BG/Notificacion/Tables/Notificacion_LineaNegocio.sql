CREATE TABLE [Notificacion].[Notificacion_LineaNegocio] (
    [Cod_Notificacion] INT         NOT NULL,
    [Cod_Linea]        VARCHAR (1) NOT NULL,
    [Fecha]            DATE        NULL,
    CONSTRAINT [PK_Notificacion_LineaNegocio] PRIMARY KEY CLUSTERED ([Cod_Notificacion] ASC, [Cod_Linea] ASC),
    CONSTRAINT [fk_LinNotificacion] FOREIGN KEY ([Cod_Notificacion]) REFERENCES [Notificacion].[Notificacion] ([Codigo])
);

