CREATE TABLE [Proveedor].[Pro_Estado_Proveedor] (
    [CodProveedor]  VARCHAR (10) NOT NULL,
    [Estado]        VARCHAR (2)  NOT NULL,
    [FechaRegistro] DATETIME     NOT NULL,
    CONSTRAINT [PK_Pro_Estado_Proveedor] PRIMARY KEY CLUSTERED ([CodProveedor] ASC, [FechaRegistro] ASC),
    CONSTRAINT [FK_Estado_Prov_Proveedor] FOREIGN KEY ([CodProveedor]) REFERENCES [Proveedor].[Pro_Proveedor] ([CodProveedor])
);

