CREATE TABLE [Notificacion].[Notificacion_Departamento] (
    [Cod_Notificacion] INT          NOT NULL,
    [Cod_Departamento] VARCHAR (10) NOT NULL,
    [Fecha]            DATE         NULL,
    CONSTRAINT [PK_Notificacion_Departamento] PRIMARY KEY CLUSTERED ([Cod_Notificacion] ASC, [Cod_Departamento] ASC),
    CONSTRAINT [fk_DepNotificacion] FOREIGN KEY ([Cod_Notificacion]) REFERENCES [Notificacion].[Notificacion] ([Codigo])
);

