CREATE TABLE [Proveedor].[Pro_MedioContacto] (
    [IdContacto]         BIGINT        NULL,
    [IdMedioContacto]    BIGINT        IDENTITY (1, 1) NOT NULL,
    [CodProveedor]       VARCHAR (10)  NULL,
    [TipMedioContacto]   VARCHAR (10)  NULL,
    [ValorMedioContacto] VARCHAR (100) NULL,
    [Estado]             BIT           NULL,
    PRIMARY KEY CLUSTERED ([IdMedioContacto] ASC),
    CONSTRAINT [FK_PRO_M_REFERENCE_PRO_C] FOREIGN KEY ([IdContacto]) REFERENCES [Proveedor].[Pro_Contacto] ([Id]),
    CONSTRAINT [FK_PRO_M_REFERENCE_PRO_P] FOREIGN KEY ([CodProveedor]) REFERENCES [Proveedor].[Pro_Proveedor] ([CodProveedor])
);


GO
CREATE NONCLUSTERED INDEX [ix_Pro_IdMedioContacto_01]
    ON [Proveedor].[Pro_MedioContacto]([IdMedioContacto] ASC);

