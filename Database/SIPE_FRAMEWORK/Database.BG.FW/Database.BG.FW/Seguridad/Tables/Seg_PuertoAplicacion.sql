CREATE TABLE [Seguridad].[Seg_PuertoAplicacion] (
    [IdAplicacion]   INT           NOT NULL,
    [Puerto]         INT           NOT NULL,
    [IdRol]          INT           NOT NULL,
    [QueueIN]        VARCHAR (100) NOT NULL,
    [QueueOUT]       VARCHAR (100) NOT NULL,
    [MaxHilos]       INT           NOT NULL,
    [BackLog]        INT           NOT NULL,
    [Separador]      VARCHAR (5)   NOT NULL,
    [MaxReadIdleMs]  INT           NOT NULL,
    [MaxSendIdleMs]  INT           NOT NULL,
    [TipoTrama]      VARCHAR (50)  NOT NULL,
    [PosTransaccion] INT           NULL,
    CONSTRAINT [PK_PuertoAplicacion] PRIMARY KEY CLUSTERED ([IdAplicacion] ASC, [Puerto] ASC) ON [Seguridad],
    CONSTRAINT [FK_PuertoAplicacion_Aplicacion] FOREIGN KEY ([IdAplicacion]) REFERENCES [Seguridad].[Seg_Aplicacion] ([IdAplicacion]),
    CONSTRAINT [FK_PuertoAplicacion_Rol] FOREIGN KEY ([IdRol]) REFERENCES [Seguridad].[Seg_Rol] ([IdRol])
) ON [Seguridad];

