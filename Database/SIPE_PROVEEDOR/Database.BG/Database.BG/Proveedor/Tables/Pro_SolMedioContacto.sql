CREATE TABLE [Proveedor].[Pro_SolMedioContacto] (
    [IdSolContacto]      BIGINT        NULL,
    [IdMedioContacto]    BIGINT        IDENTITY (1, 1) NOT NULL,
    [IdSolicitud]        BIGINT        NULL,
    [TipMedioContacto]   VARCHAR (10)  NULL,
    [ValorMedioContacto] VARCHAR (100) NULL,
    [Estado]             BIT           NULL,
    CONSTRAINT [PK_PRO_SOLMEDIOCONTACTO] PRIMARY KEY CLUSTERED ([IdMedioContacto] ASC),
    CONSTRAINT [FK_PRO_SOLM_REFERENCE_PRO_SOLC] FOREIGN KEY ([IdSolContacto]) REFERENCES [Proveedor].[Pro_SolContacto] ([IdSolContacto]),
    CONSTRAINT [FK_PRO_SOLM_REFERENCE_PRO_SOLP] FOREIGN KEY ([IdSolicitud]) REFERENCES [Proveedor].[Pro_SolProveedor] ([IdSolicitud])
);

