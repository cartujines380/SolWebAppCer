CREATE TABLE [Proveedor].[Pro_ConPagos] (
    [Id]                 INT             IDENTITY (1, 1) NOT NULL,
    [TipoIdentificacion] VARCHAR (1)     NOT NULL,
    [Identificacion]     VARCHAR (13)    NOT NULL,
    [CodProveedorAx]     VARCHAR (40)    NOT NULL,
    [Factura]            VARCHAR (200)   NOT NULL,
    [FormaPago]          VARCHAR (5)     NOT NULL,
    [FechaPago]          DATETIME        NOT NULL,
    [Valor]              NUMERIC (13, 2) NOT NULL,
    [Detalle]            VARCHAR (100)   NOT NULL,
    [FechaCreacion]      DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [idx1]
    ON [Proveedor].[Pro_ConPagos]([Identificacion] ASC);


GO
CREATE NONCLUSTERED INDEX [idx2]
    ON [Proveedor].[Pro_ConPagos]([CodProveedorAx] ASC);


GO
CREATE NONCLUSTERED INDEX [idx3]
    ON [Proveedor].[Pro_ConPagos]([Factura] ASC);


GO
CREATE NONCLUSTERED INDEX [idx4]
    ON [Proveedor].[Pro_ConPagos]([FechaPago] ASC);

