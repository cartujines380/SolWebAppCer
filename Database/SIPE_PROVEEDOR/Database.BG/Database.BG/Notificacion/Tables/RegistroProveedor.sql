CREATE TABLE [Notificacion].[RegistroProveedor] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [Nombre]    VARCHAR (60)  NULL,
    [Correo]    VARCHAR (60)  NULL,
    [Telefono]  VARCHAR (20)  NULL,
    [Celular]   VARCHAR (20)  NULL,
    [Productos] VARCHAR (200) NULL,
    [Fecha]     DATETIME      NULL,
    CONSTRAINT [PK_IdNuevoProv] PRIMARY KEY CLUSTERED ([Id] ASC)
);

