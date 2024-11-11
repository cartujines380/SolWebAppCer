CREATE TABLE [Seguridad].[Seg_ServAplicacion] (
    [IdAplicacion] INT          NOT NULL,
    [IdServidor]   INT          NOT NULL,
    [TipoServidor] VARCHAR (10) NOT NULL,
    CONSTRAINT [PK_ServAplicacion] PRIMARY KEY CLUSTERED ([IdAplicacion] ASC, [IdServidor] ASC) ON [Seguridad],
    CONSTRAINT [FK_ServAplicacion_Aplicacion] FOREIGN KEY ([IdAplicacion]) REFERENCES [Seguridad].[Seg_Aplicacion] ([IdAplicacion]),
    CONSTRAINT [FK_ServAplicacion_Aplicacion1] FOREIGN KEY ([IdServidor]) REFERENCES [Seguridad].[Seg_Aplicacion] ([IdAplicacion])
) ON [Seguridad];

