CREATE TABLE [Notificacion].[Not_DepFuncion] (
    [Cod_Notificacion] INT          NOT NULL,
    [CodDepartamento]  VARCHAR (10) NOT NULL,
    [CodFuncion]       VARCHAR (10) NOT NULL,
    [Fecha]            DATETIME     NOT NULL,
    CONSTRAINT [PK_Not_DepFuncion_1] PRIMARY KEY CLUSTERED ([Cod_Notificacion] ASC, [CodDepartamento] ASC, [CodFuncion] ASC),
    CONSTRAINT [FK_Not_DepFuncion_Not] FOREIGN KEY ([Cod_Notificacion]) REFERENCES [Notificacion].[Notificacion] ([Codigo])
);

