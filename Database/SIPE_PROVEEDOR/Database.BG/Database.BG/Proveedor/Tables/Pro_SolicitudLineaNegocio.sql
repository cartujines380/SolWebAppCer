CREATE TABLE [Proveedor].[Pro_SolicitudLineaNegocio] (
    [IdSolicitud]  BIGINT       NOT NULL,
    [LineaNegocio] VARCHAR (10) NOT NULL,
    [Principal]    BIT          NOT NULL,
    CONSTRAINT [PK_Pro_SolicitudLineaNegocio] PRIMARY KEY CLUSTERED ([IdSolicitud] ASC, [LineaNegocio] ASC),
    CONSTRAINT [FK_LNegocio_SProv] FOREIGN KEY ([IdSolicitud]) REFERENCES [Proveedor].[Pro_SolProveedor] ([IdSolicitud])
);

