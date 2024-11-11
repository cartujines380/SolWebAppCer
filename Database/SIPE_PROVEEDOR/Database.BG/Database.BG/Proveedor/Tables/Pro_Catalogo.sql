CREATE TABLE [Proveedor].[Pro_Catalogo] (
    [Tabla]       INT           NOT NULL,
    [Codigo]      VARCHAR (20)  NOT NULL,
    [Detalle]     VARCHAR (MAX) NULL,
    [Estado]      CHAR (1)      NOT NULL,
    [DescAlterno] VARCHAR (150) NULL,
    CONSTRAINT [pk_catalogo] PRIMARY KEY CLUSTERED ([Tabla] ASC, [Codigo] ASC) ON [Proveedor]
) ON [Proveedor] TEXTIMAGE_ON [Proveedor];

