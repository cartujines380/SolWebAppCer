CREATE TABLE [Seguridad].[Seg_HistParamAplicacion] (
    [IdAplicacion] INT           NOT NULL,
    [Parametro]    VARCHAR (50)  NOT NULL,
    [FechaAct]     DATETIME      NOT NULL,
    [Valor]        VARCHAR (200) NOT NULL,
    [Encriptado]   BIT           NULL,
    CONSTRAINT [PK_HistParamAplicacion] PRIMARY KEY CLUSTERED ([IdAplicacion] ASC, [Parametro] ASC, [FechaAct] ASC) ON [Seguridad],
    CONSTRAINT [FK_ParamAplicacion_HistParamAplicacion] FOREIGN KEY ([IdAplicacion], [Parametro]) REFERENCES [Seguridad].[Seg_ParamAplicacion] ([IdAplicacion], [Parametro])
) ON [Seguridad];

