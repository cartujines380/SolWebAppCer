CREATE TABLE [Pedidos].[Red_RequerimientosAdjunto] (
    [idadjunto]       BIGINT        IDENTITY (1, 1) NOT NULL,
    [idrequerimiento] BIGINT        NOT NULL,
    [nombrearchivo]   NCHAR (100)   NULL,
    [descripcion]     VARCHAR (200) NULL,
    [tipo]            CHAR (1)      NULL,
    CONSTRAINT [PK_Red_RequerimientosAdjunto] PRIMARY KEY CLUSTERED ([idadjunto] ASC),
    CONSTRAINT [FK_RequAdj_Requ] FOREIGN KEY ([idrequerimiento]) REFERENCES [Pedidos].[Red_Requerimientos] ([idrequerimiento])
);

