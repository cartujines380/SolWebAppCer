CREATE TABLE [Seguridad].[Seg_ParamAplicacion] (
    [IdAplicacion] INT           NOT NULL,
    [Parametro]    VARCHAR (50)  NOT NULL,
    [Valor]        VARCHAR (200) NOT NULL,
    [Encriptado]   BIT           NULL,
    CONSTRAINT [PK_ParamAplicacion] PRIMARY KEY CLUSTERED ([IdAplicacion] ASC, [Parametro] ASC) ON [Seguridad],
    CONSTRAINT [FK_Aplicacion_ParamAplicacion] FOREIGN KEY ([IdAplicacion]) REFERENCES [Seguridad].[Seg_Aplicacion] ([IdAplicacion])
) ON [Seguridad];


GO
CREATE NONCLUSTERED INDEX [IDX_ParamAplicacion_001]
    ON [Seguridad].[Seg_ParamAplicacion]([IdAplicacion] ASC)
    ON [Indices];

