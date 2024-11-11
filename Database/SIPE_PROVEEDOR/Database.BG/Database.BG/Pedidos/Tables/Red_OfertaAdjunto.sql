CREATE TABLE [Pedidos].[Red_OfertaAdjunto] (
    [idadjunto]     BIGINT        IDENTITY (1, 1) NOT NULL,
    [idAdquisicion] BIGINT        NOT NULL,
    [codProveedor]  VARCHAR (10)  NOT NULL,
    [nombrearchivo] NCHAR (100)   NULL,
    [descripcion]   VARCHAR (200) NULL,
    CONSTRAINT [PK_Red_RequerimientosOfertaAdjunto] PRIMARY KEY CLUSTERED ([idadjunto] ASC),
    CONSTRAINT [FK_OferAdq_Req] FOREIGN KEY ([idAdquisicion]) REFERENCES [Pedidos].[Red_Adquisicion] ([idAdquisicion])
);

