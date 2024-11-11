CREATE TABLE [Catalogo].[Ctl_Catalogo] (
    [IdTabla]     INT           NOT NULL,
    [Codigo]      VARCHAR (50)  NOT NULL,
    [Descripcion] VARCHAR (MAX) NULL,
    [DescAlterno] VARCHAR (150) NULL,
    [Estado]      CHAR (1)      NULL,
    [Necesario]   BIT           NULL,
    [Relacion1]   VARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([IdTabla] ASC, [Codigo] ASC) ON [Catalogo],
    CONSTRAINT [FK_Tabla_Catalogo] FOREIGN KEY ([IdTabla]) REFERENCES [Catalogo].[Ctl_Tabla] ([IdTabla])
) ON [Catalogo] TEXTIMAGE_ON [Catalogo];


GO
CREATE NONCLUSTERED INDEX [IDX_Catalogo_001]
    ON [Catalogo].[Ctl_Catalogo]([IdTabla] ASC)
    ON [Indices];

