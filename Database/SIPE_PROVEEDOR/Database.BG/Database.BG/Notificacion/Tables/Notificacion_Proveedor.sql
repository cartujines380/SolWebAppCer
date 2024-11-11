CREATE TABLE [Notificacion].[Notificacion_Proveedor] (
    [Cod_Notificacion] INT          NOT NULL,
    [Cod_proveedor]    VARCHAR (10) NOT NULL,
    [FecAceptacion]    DATETIME     NULL,
    [Estado]           CHAR (1)     NOT NULL,
    [Usuario]          VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_Notificacion_Proveedor_1] PRIMARY KEY CLUSTERED ([Cod_Notificacion] ASC, [Cod_proveedor] ASC, [Usuario] ASC),
    CONSTRAINT [fk_ProvNotificacion] FOREIGN KEY ([Cod_Notificacion]) REFERENCES [Notificacion].[Notificacion] ([Codigo])
);

