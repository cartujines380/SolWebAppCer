CREATE TABLE [Seguridad].[Seg_RespuestaSegura] (
    [IdParticipante] INT          NOT NULL,
    [CodPregunta]    VARCHAR (10) NOT NULL,
    [Respuesta]      VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_SegRespuestaSegura] PRIMARY KEY CLUSTERED ([IdParticipante] ASC, [CodPregunta] ASC) ON [Participante]
) ON [Participante];

