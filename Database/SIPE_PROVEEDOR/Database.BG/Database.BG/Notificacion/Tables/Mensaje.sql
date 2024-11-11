CREATE TABLE [Notificacion].[Mensaje] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Usuario]  VARCHAR (80)  NULL,
    [Correo]   VARCHAR (80)  NULL,
    [Mensaje]  VARCHAR (MAX) NULL,
    [Fecha]    DATETIME      NULL,
    [Telefono] VARCHAR (20)  NULL,
    CONSTRAINT [PK_Mensaje] PRIMARY KEY CLUSTERED ([Id] ASC)
);

