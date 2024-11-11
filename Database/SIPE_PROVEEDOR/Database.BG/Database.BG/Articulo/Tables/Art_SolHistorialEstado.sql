CREATE TABLE [Articulo].[Art_SolHistorialEstado] (
    [IdHistorialEstado] BIGINT        IDENTITY (1, 1) NOT NULL,
    [IdDetalle]         BIGINT        NULL,
    [IdSolicitud]       BIGINT        NOT NULL,
    [Motivo]            VARCHAR (10)  NULL,
    [Observacion]       VARCHAR (500) NULL,
    [Usuario]           VARCHAR (100) NULL,
    [Fecha]             DATETIME      NULL,
    [EstadoArticulo]    VARCHAR (10)  NULL,
    [EstadoSolicitud]   VARCHAR (10)  NULL,
    CONSTRAINT [PK_ART_SOLESTADOS] PRIMARY KEY CLUSTERED ([IdHistorialEstado] ASC),
    CONSTRAINT [FK_HistEstado_Detalle] FOREIGN KEY ([IdDetalle], [IdSolicitud]) REFERENCES [Articulo].[Art_SolDetalle] ([IdDetalle], [IdSolicitud])
);

