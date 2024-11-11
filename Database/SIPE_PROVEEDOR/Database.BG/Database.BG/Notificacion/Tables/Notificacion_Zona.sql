CREATE TABLE [Notificacion].[Notificacion_Zona] (
    [Cod_Notificacion] INT          NOT NULL,
    [Cod_Zona]         VARCHAR (12) NOT NULL,
    [Fecha]            DATE         NULL,
    CONSTRAINT [PK_Notificacion_Zona] PRIMARY KEY CLUSTERED ([Cod_Notificacion] ASC, [Cod_Zona] ASC),
    CONSTRAINT [fk_ZonNotificacion] FOREIGN KEY ([Cod_Notificacion]) REFERENCES [Notificacion].[Notificacion] ([Codigo])
);

