CREATE TABLE [Seguridad].[Seg_Clave] (
    [IdParticipante]      INT          NOT NULL,
    [EsClaveNuevo]        VARCHAR (1)  NOT NULL,
    [EsClaveCambio]       VARCHAR (1)  NOT NULL,
    [EsClaveBloqueo]      VARCHAR (1)  NOT NULL,
    [FechaUltModClave]    DATETIME     NOT NULL,
    [FechaUltClaveErr]    DATETIME     NOT NULL,
    [NumIntentosClaveErr] SMALLINT     NOT NULL,
    [ImagenSecreta]       VARCHAR (10) NOT NULL,
    CONSTRAINT [PK_SegClave] PRIMARY KEY CLUSTERED ([IdParticipante] ASC) ON [Participante]
) ON [Participante];

