CREATE TABLE [Proveedor].[Pro_Tabla] (
    [Tabla]       INT          NOT NULL,
    [TablaNombre] VARCHAR (30) NOT NULL,
    [Estado]      CHAR (1)     NOT NULL,
    PRIMARY KEY CLUSTERED ([Tabla] ASC) ON [Proveedor]
) ON [Proveedor];

