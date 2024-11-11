CREATE TABLE [Pedidos].[Red_Requerimientos] (
    [idrequerimiento]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [fecharequerimiento] DATE           NOT NULL,
    [codcategoria]       NCHAR (10)     NOT NULL,
    [codempresa]         NCHAR (10)     NOT NULL,
    [monto]              VARCHAR (100)  NULL,
    [descripcion]        VARCHAR (8000) NULL,
    [estado]             NCHAR (10)     NULL,
    [usuariocreacion]    NCHAR (10)     NULL,
    [fechacreacion]      DATE           NULL,
    [titulo]             VARCHAR (100)  NULL,
    CONSTRAINT [PK_Red_Requerimientos] PRIMARY KEY CLUSTERED ([idrequerimiento] ASC)
);

