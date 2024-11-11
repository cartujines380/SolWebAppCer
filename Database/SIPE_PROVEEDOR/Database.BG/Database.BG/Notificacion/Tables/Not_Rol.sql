CREATE TABLE [Notificacion].[Not_Rol] (
    [Cod_Notificacion] INT          NOT NULL,
    [CodRol]           VARCHAR (20) NOT NULL,
    [FechaRegistro]    DATETIME     NOT NULL,
    CONSTRAINT [PK_Not_Rol_1] PRIMARY KEY CLUSTERED ([Cod_Notificacion] ASC, [CodRol] ASC),
    CONSTRAINT [FK_Not_Rol_Not] FOREIGN KEY ([Cod_Notificacion]) REFERENCES [Notificacion].[Notificacion] ([Codigo])
);

