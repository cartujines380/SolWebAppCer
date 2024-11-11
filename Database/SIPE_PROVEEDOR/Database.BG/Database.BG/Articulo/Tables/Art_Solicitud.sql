CREATE TABLE [Articulo].[Art_Solicitud] (
    [IdSolicitud]     BIGINT        IDENTITY (1, 1) NOT NULL,
    [TipoSolicitud]   VARCHAR (10)  NULL,
    [LineaNegocio]    VARCHAR (10)  NULL,
    [CodProveedor]    VARCHAR (10)  NOT NULL,
    [Fecha]           DATETIME      NULL,
    [EstadoSolicitud] VARCHAR (10)  NULL,
    [Usuario]         VARCHAR (100) NULL,
    CONSTRAINT [PK_ART_SOLARTICULO] PRIMARY KEY CLUSTERED ([IdSolicitud] ASC)
);

