CREATE TABLE [Seguridad].[Seg_Registro] (
    [IdUsuario]        VARCHAR (20)  NOT NULL,
    [Token]            VARCHAR (50)  NOT NULL,
    [IdIdentificacion] VARCHAR (20)  NOT NULL,
    [IdAplicacion]     INT           NOT NULL,
    [IdEmpresa]        INT           NOT NULL,
    [FechaIngreso]     DATETIME      NOT NULL,
    [FechaSalida]      DATETIME      NULL,
    [Estado]           CHAR (1)      NULL,
    [FechaUltTrans]    DATETIME      NULL,
    [Dominio]          VARCHAR (100) NULL,
    [MacMaquina]       VARCHAR (20)  NULL,
    CONSTRAINT [PK_Registro] PRIMARY KEY CLUSTERED ([IdUsuario] ASC, [Token] ASC, [IdIdentificacion] ASC) ON [Seguridad],
    CONSTRAINT [FK_Aplicacion_Registro] FOREIGN KEY ([IdAplicacion]) REFERENCES [Seguridad].[Seg_Aplicacion] ([IdAplicacion])
) ON [Seguridad];


GO
CREATE NONCLUSTERED INDEX [IDX_Registro_001]
    ON [Seguridad].[Seg_Registro]([IdAplicacion] ASC)
    ON [Indices];


GO
CREATE NONCLUSTERED INDEX [IDX_Registro_002]
    ON [Seguridad].[Seg_Registro]([IdUsuario] ASC, [FechaIngreso] ASC)
    ON [Indices];

