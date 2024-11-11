CREATE TABLE [Participante].[Par_Participante] (
    [IdParticipante]       INT           IDENTITY (1, 1) NOT NULL,
    [IdTipoIdentificacion] VARCHAR (10)  NOT NULL,
    [Identificacion]       VARCHAR (100) NOT NULL,
    [TipoParticipante]     CHAR (1)      NOT NULL,
    [IdUsuario]            VARCHAR (50)  NOT NULL,
    [Estado]               VARCHAR (10)  NULL,
    [FechaRegistro]        DATETIME      NULL,
    [IdPais]               VARCHAR (10)  NULL,
    [IdProvincia]          VARCHAR (10)  NULL,
    [IdCiudad]             VARCHAR (10)  NULL,
    [CuentaContable]       VARCHAR (50)  NULL,
    [FechaExpira]          DATETIME      NULL,
    [TiempoExpira]         INT           NULL,
    [Comentario]           VARCHAR (255) NULL,
    [ChequeaEquipo]        BIT           NULL,
    [Opident]              VARCHAR (50)  NULL,
    [IdNaturalezaNegocio]  VARCHAR (10)  NULL,
    [FechaConstitucion]    DATETIME      NULL,
    [TipoPartRegistro]     CHAR (1)      NULL,
    [Clave]                VARCHAR (300) NULL,
    CONSTRAINT [PK_Participante] PRIMARY KEY CLUSTERED ([IdParticipante] ASC) ON [Participante]
) ON [Participante];


GO
CREATE NONCLUSTERED INDEX [IDX_Participante_001]
    ON [Participante].[Par_Participante]([IdTipoIdentificacion] ASC, [Identificacion] ASC)
    ON [Indices];


GO
CREATE NONCLUSTERED INDEX [IDX_Participante_002]
    ON [Participante].[Par_Participante]([IdUsuario] ASC)
    ON [Indices];

