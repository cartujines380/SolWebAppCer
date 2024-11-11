CREATE TABLE [Proveedor].[Pro_PagosBitacora] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Proceso]       VARCHAR (25)   NULL,
    [FechaRegistro] DATETIME       NULL,
    [Servicio]      VARCHAR (25)   NULL,
    [Detalle]       VARCHAR (5000) NULL,
    [Accion]        VARCHAR (5)    NULL,
    [Estado]        VARCHAR (1)    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [idx_fecha_registro]
    ON [Proveedor].[Pro_PagosBitacora]([FechaRegistro] ASC);


GO
CREATE NONCLUSTERED INDEX [idx_detalle]
    ON [Proveedor].[Pro_PagosBitacora]([Detalle] ASC);

