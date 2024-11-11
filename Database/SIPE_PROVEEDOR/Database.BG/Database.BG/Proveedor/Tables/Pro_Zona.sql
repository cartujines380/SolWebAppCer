CREATE TABLE [Proveedor].[Pro_Zona] (
    [CodZona]     VARCHAR (2)   NOT NULL,
    [Descripcion] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Pro_Zona] PRIMARY KEY CLUSTERED ([CodZona] ASC) ON [Proveedor]
) ON [Proveedor];

